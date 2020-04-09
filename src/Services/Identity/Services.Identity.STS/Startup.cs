using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

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
            ConfigureEntityFrameworkService(services);

            ConfigureIdentityServerService(services);

            services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = !WebHostEnvironment.IsProduction());
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
    
        #region Configure Services

        private void ConfigureEntityFrameworkService(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Sql");

            services.AddDbContext<Application.Data.ApplicationDbContext>(options =>
            options.UseMySql(connectionString, mySqlOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            }));
        }

        private void ConfigureIdentityServerService(IServiceCollection services)
        {
            services.AddIdentity<Application.Models.ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Application.Data.ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
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
                .AddAspNetIdentity<Application.Models.ApplicationUser>();
            
            services.AddTransient<IdentityServer4.Services.IProfileService, Application.Services.ProfileService>();
            services.AddTransient<Application.Services.ILoginService, Application.Services.LoginService>();
        }

        #endregion Configure Services
    }
}
