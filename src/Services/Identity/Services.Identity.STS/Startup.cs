using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using Transversal.Common.InversionOfControl.Registrar;
using Transversal.Web;

namespace Services.Identity.STS
{
    public class Startup : WebBootstrapperBase<Settings>
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        {
        }

        public override IIoCRegistrarCatalog GetIoCRegistrarCatalog()
        {
            var catalog = new IoCRegistrarCatalog();
            catalog.Register<Core.Data.DataIoCRegistrar>();
            catalog.Register<Core.Application.ApplicationIoCRegistrar>();
            return catalog;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServiceCollection(services);

            ConfigureIdentityServerService(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
        private void ConfigureIdentityServerService(IServiceCollection services)
        {
            services
                .AddIdentityCore<Core.Domain.Users.User>()
                .AddDefaultTokenProviders();

            // https://github.com/dotnet/aspnetcore/blob/3.0/src/Identity/Core/src/IdentityServiceCollectionExtensions.cs
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                o.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                };
            })
            .AddCookie(IdentityConstants.ExternalScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.ExternalScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                o.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                };
            })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            services.AddAuthorization();

            var certificate = Certificates.CertificatesHelper.GetForSigningCredential(_webHostEnvironment, _configuration);
            var clientEndpoints = _configuration.GetSection("ClientEndpoints").Get<Dictionary<string, string>>();

            services
                .AddIdentityServer(x =>
                {
                    x.IssuerUri = "null";
                    x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                })
                .AddSigningCredential(certificate)
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResourcesConfig.GetResources())
                .AddInMemoryApiResources(IdentityServerConfig.ApiResourcesConfig.GetApis())
                .AddInMemoryApiScopes(IdentityServerConfig.ApiResourcesConfig.GetApiScopes())
                .AddInMemoryClients(IdentityServerConfig.ClientsConfig.GetClients(clientEndpoints))
                .AddAspNetIdentity<Core.Domain.Users.User>();

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers?view=aspnetcore-3.1
            services.TryAddScoped<IdentityErrorDescriber>();

            //services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<Core.Domain.Users.User>>();
            //services.TryAddScoped<IUserClaimsPrincipalFactory<Core.Domain.Users.User>, UserClaimsPrincipalFactory<Core.Domain.Users.User>>();
            services.TryAddScoped<UserManager<Core.Domain.Users.User>>();
            services.TryAddScoped<SignInManager<Core.Domain.Users.User>>();

            services.AddTransient<IUserStore<Core.Domain.Users.User>, Core.Data.Repositories.UserRepository>();
            services.AddTransient<IdentityServer4.Services.IProfileService, Core.Application.CustomIdentity.CustomProfileService>();
            services.AddTransient<ISecurityStampValidator, Core.Application.CustomIdentity.CustomSecurityStampValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            ConfigureApplication(app);

            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            // Make work identity server redirections in Edge and lastest versions of browers.
            // WARN: Not valid in a production environment.
            if (_webHostEnvironment.IsDevelopment())
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("Content-Security-Policy", "script-src 'self' 'unsafe-inline'");
                    await next();
                });
            }

            app.UseForwardedHeaders();

            app.UseIdentityServer();

            // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
            // the coockies shold be expided from https, but in eShop, the internal comunicacion in aks and docker compose is http.
            // To avoid this problem, the policy of cookies shold be in Lax mode.
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
            });
        }
        
        protected override void OnBootstrapCompleted()
        {
            base.OnBootstrapCompleted();

            var defaultDbMigrator = _ioCManager.Resolve<Transversal.Data.EFCore.DbMigrator.IEFCoreDbMigrator<Core.Data.DefaultDbContext>>();
            if (defaultDbMigrator != null)
            {
                defaultDbMigrator.CreateOrMigrate();
            }
        }
    }
}
