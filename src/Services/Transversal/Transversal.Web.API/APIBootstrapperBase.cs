using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using Transversal.Core;
using Transversal.Web.API.Swagger;

namespace Transversal.Web.API
{
    public abstract class APIBootstrapperBase<TAPIBootstrapperSettings> : WebBootstrapperBase<TAPIBootstrapperSettings>
        where TAPIBootstrapperSettings : APIBootstrapperSettingsBase, IBootstrapperSettings, new()
    {
        #region Fields

        protected TAPIBootstrapperSettings _apiBootstrapperSettings => ((TAPIBootstrapperSettings)_bootstrapperSettings);

        #endregion Fields

        #region Ctor

        public APIBootstrapperBase(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
            : base(configuration, webHostEnvironment)
        {
        }

        #endregion Ctor

        #region Methods

        #region Configure service collection

        public override void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            base.ConfigureServiceCollection(serviceCollection);

            serviceCollection
                .AddControllers(options => options.Filters.Add(typeof(Filters.HttpGlobalExceptionFilter)))
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = _apiBootstrapperSettings.WriteIndentedJson);

            AddSwaggerToServiceCollection(serviceCollection);
        }

        protected override void AddAuthenticationToServiceCollecion(IServiceCollection serviceCollection)
        {
            base.AddAuthenticationToServiceCollecion(serviceCollection);

            if (_webHostEnvironment.IsDevelopment())
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            // Prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            serviceCollection
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = _apiBootstrapperSettings.Authentication.Authority;
                    options.Audience = _apiBootstrapperSettings.Authentication.Audience;
                    options.RequireHttpsMetadata = _apiBootstrapperSettings.Authentication.RequireHttpsMetadata;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
        }

        protected virtual void AddSwaggerToServiceCollection(IServiceCollection serviceCollection)
        {
            if (_apiBootstrapperSettings.Swagger.IsEnabled)
            {
                SwaggerExtensions.AddSwagger(serviceCollection
                    , settings: _apiBootstrapperSettings.Swagger);
            }
        }

        #endregion

        #region Configure application

        public override void ConfigureApplication(IApplicationBuilder app, Action<IEndpointRouteBuilder> onEndpointRouteBuilder = null)
        {
            base.ConfigureApplication(app, onEndpointRouteBuilder);

            ConfigureSwagger(app);

            app
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapControllers();

                    onEndpointRouteBuilder?.Invoke(endpoints);
                });
        }

        protected virtual void ConfigureSwagger(IApplicationBuilder app)
        {
            if (_apiBootstrapperSettings.Swagger.IsEnabled)
            {
                _logger.LogDebug("Configuring Swagger...");
                SwaggerExtensions.UseSwagger(app
                    , settings: _apiBootstrapperSettings.Swagger
                    , pathBase: _apiBootstrapperSettings.ASPNet.PathBase ?? string.Empty);
            }
        }

        #endregion

        #endregion Methods
    }
}
