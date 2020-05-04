using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Autofac;

namespace Services.Localization.API
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
                .AddControllers(options => options.Filters.Add(typeof(Transversal.Web.API.Filters.HttpGlobalExceptionFilter)))
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = !WebHostEnvironment.IsProduction());

            ConfigureAuthenticationService(services);

            ConfigureSwaggerService(services);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<Startup>();

            #region Base path
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                logger.LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }
            #endregion Base path

            app.UseSerilogRequestLogging();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            ConfigureAuthentication(app, logger);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            ConfigureSwagger(app, pathBase, logger);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            DI.Register(builder);

            builder.RegisterType<Transversal.Web.Dependency.AutofacIocManager>().As<Transversal.Common.Dependency.IIocManager>();
            builder.RegisterType<Transversal.Web.Dependency.AutofacIocManager>().As<Transversal.Common.Dependency.IIocResolver>();
        }

        #region Configure Services

        private void ConfigureAuthenticationService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityUrl");
                options.Audience = "localization";
                options.RequireHttpsMetadata = false;
            });
        }

        private void ConfigureSwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "microservicesdotnet - Localization HTTP API",
                    Version = "v1",
                    Description = "The Localization Microservice HTTP API.",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "localization", "Localizationn API" }
                            }
                        }
                    }
                });

                options.OperationFilter<Transversal.Web.API.Swagger.Filters.AuthorizeCheckOperationFilter>();
            });
        }

        #endregion Configure Services

        #region Configure

        private void ConfigureAuthentication(IApplicationBuilder app, ILogger<Startup> logger)
        {
            logger.LogDebug("Configuring Authentication...");

            app.UseAuthentication();
            app.UseAuthorization();
        }

        private void ConfigureSwagger(IApplicationBuilder app, string pathBase, ILogger<Startup> logger)
        {
            logger.LogDebug("Configuring Swagger...");

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Localization.API V1");
                    c.OAuthClientId("localizationswaggerui");
                    c.OAuthAppName("Localization Swagger UI");
                });
        }

        #endregion Configure
    }

    public static class DI
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Transversal.Web.Session.SessionInfo>()
                .As<Transversal.Common.Session.ISessionInfo>();

            containerBuilder.RegisterType<Core.Data.Repositories.ResourceGroupRepository>()
                .As<Core.Domain.Resources.IResourceGroupRepository>();
            containerBuilder.RegisterType<Core.Application.ResourceGroupsAppService>()
                .As<Core.Application.IResourceGroupsAppService>();

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
            containerBuilder.RegisterType<Transversal.Data.EFCore.DbContext.DefaultDbContextInterceptor>()
                .As<Transversal.Data.EFCore.DbContext.IDbContextInterceptor>();
            containerBuilder.RegisterGeneric(typeof(Transversal.Data.EFCore.Uow.UnitOfWorkDbContextProvider<>))
                .As(typeof(Transversal.Data.EFCore.DbContext.IDbContextProvider<>));
            containerBuilder.RegisterType<Transversal.Data.EFCore.Uow.DbContextEfCoreTransactionStrategy>()
                .As<Transversal.Data.EFCore.Uow.IEfCoreTransactionStrategy>();
        }
    }
}
