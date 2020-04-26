using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace Services.Identity.STS
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = !WebHostEnvironment.IsProduction());

            ConfigureIdentityServerService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<Startup>();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Base path
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                logger.LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }
            #endregion Base path

            app.UseSerilogRequestLogging();
            
            app.UseStaticFiles();

            // Make work identity server redirections in Edge and lastest versions of browers.
            // WARN: Not valid in a production environment.
            if (env.IsDevelopment())
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

        // This method gets called by the runtime. Use this method configure the dependency injection container.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<Transversal.Web.Dependency.AutofacIocManager>().As<Transversal.Common.Dependency.IIocManager>();
            builder.RegisterType<Transversal.Web.Dependency.AutofacIocManager>().As<Transversal.Common.Dependency.IIocResolver>();

            DI.Register(builder);
        }

        #region Configure Services

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

            services.AddHttpContextAccessor();

            var certificate = Certificates.CertificatesHelper.Get(WebHostEnvironment, Configuration);
            var clientEndpoints = Configuration.GetSection("ClientEndpoints").Get<Dictionary<string, string>>();

            services
                .AddIdentityServer(x =>
                {
                    x.IssuerUri = "null";
                    x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                })
                .AddSigningCredential(certificate)
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResourcesConfig.GetResources())
                .AddInMemoryApiResources(IdentityServerConfig.ApiResourcesConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.ClientsConfig.GetClients(clientEndpoints))
                .AddAspNetIdentity<Core.Domain.Users.User>();

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers?view=aspnetcore-3.1
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<Core.Domain.Users.User>>();
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<Core.Domain.Users.User>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<Core.Domain.Users.User>, UserClaimsPrincipalFactory<Core.Domain.Users.User>>();
            services.TryAddScoped<UserManager<Core.Domain.Users.User>>();
            services.TryAddScoped<SignInManager<Core.Domain.Users.User>>();
            services.TryAddScoped<IUserStore<Core.Domain.Users.User>, Core.Data.Repositories.UserRepository>();
            services.TryAddScoped<IdentityServer4.Services.IProfileService, Core.Application.UsersApplicationService>();
        }

        #endregion Configure Services
    }

    public static class DI
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Transversal.Web.Session.SessionInfo>()
                .As<Transversal.Common.Session.ISessionInfo>();

            containerBuilder.RegisterType<Core.Data.Repositories.UserRepository>()
                .As<Core.Domain.Users.IUserRepository>();
            containerBuilder.RegisterType<Core.Data.Repositories.UserPictureRepository>()
                .As<Core.Domain.Users.IUserPictureRepository>();
            containerBuilder.RegisterType<Core.Application.UsersApplicationService>()
                .As<Core.Application.IUsersApplicationService>();

            containerBuilder.RegisterType<Core.Data.DefaultDbContextOptionsResolver>()
                .As<Transversal.Data.EFCore.DbContext.IDbContextOptionsResolver<Core.Data.DefaultDbContext>>();
            containerBuilder.RegisterType<Core.Data.DefaultDbContext>();
            containerBuilder.RegisterType<Core.Data.DefaultDbSeeder>()
                .As<Transversal.Data.EFCore.DbSeeder.IEfCoreDbSeeder<Core.Data.DefaultDbContext>>();

            containerBuilder.RegisterType<Transversal.Domain.Uow.Manager.UnitOfWorkManager>()
                .As<Transversal.Domain.Uow.Manager.IUnitOfWorkManager>();
            containerBuilder.RegisterType<Transversal.Domain.Uow.Options.UnitOfWorkDefaultOptions>()
                .As<Transversal.Domain.Uow.Options.IUnitOfWorkDefaultOptions>();
            containerBuilder.RegisterType<Transversal.Domain.Uow.Provider.AsyncLocalCurrentUnitOfWorkProvider>()
                .As<Transversal.Domain.Uow.Provider.ICurrentUnitOfWorkProvider>();
            containerBuilder.RegisterType<Transversal.Data.EFCore.Uow.DefaultConnectionStringResolver>()
                .As<Transversal.Domain.Uow.IConnectionStringResolver>();
            containerBuilder.RegisterType<Transversal.Data.EFCore.Uow.DefaultEfCoreUnitOfWork>()
                .As<Transversal.Domain.Uow.IUnitOfWork>();

            containerBuilder.RegisterGeneric(typeof(Transversal.Data.EFCore.DbMigrator.DefaultEFCoreDbMigrator<>))
                .As(typeof(Transversal.Data.EFCore.DbMigrator.IEFCoreDbMigrator<>));
            containerBuilder.RegisterType<Transversal.Data.EFCore.DbContext.DefaultDbContextResolver>()
                .As<Transversal.Data.EFCore.DbContext.IDbContextResolver>();
            containerBuilder.RegisterGeneric(typeof(Transversal.Data.EFCore.Uow.UnitOfWorkDbContextProvider<>))
                .As(typeof(Transversal.Data.EFCore.DbContext.IDbContextProvider<>));
            containerBuilder.RegisterType<Transversal.Data.EFCore.Uow.DbContextEfCoreTransactionStrategy>()
                .As<Transversal.Data.EFCore.Uow.IEfCoreTransactionStrategy>();
        }
    }
}
