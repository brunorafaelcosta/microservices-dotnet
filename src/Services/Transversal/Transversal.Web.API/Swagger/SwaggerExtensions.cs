using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Transversal.Web.API.Swagger
{
    /// <summary>
    /// Provides additional methods to <see cref="Swagger"/>.
    /// </summary>
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(IServiceCollection serviceCollection, SwaggerSettings settings)
        {
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(settings.DocName, new OpenApiInfo
                {
                    Title = settings.DocTitle,
                    Version = settings.DocVersion,
                    Description = settings.DocDescription,
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition(SecuritySchemeType.OAuth2.ToString().ToLower(), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(settings.OAuthAuthorizationUrl),
                            TokenUrl = new Uri(settings.OAuthTokenUrl),
                            Scopes = settings.OAuthScopes
                        }
                    }
                });

                options.OperationFilter<Filters.AuthorizeCheckOperationFilter>();
            });

            return serviceCollection;
        }

        public static IApplicationBuilder UseSwagger(IApplicationBuilder app, SwaggerSettings settings, string pathBase = null)
        {
            string endpointUrl = string.IsNullOrEmpty(pathBase)
                ? settings.EndpointUrl
                : Common.Extensions.UrlExtensions.Combine(pathBase, settings.EndpointUrl);
            string endpointName = settings.EndpointName;

            app
                .UseSwagger()
                .UseSwaggerUI(sc =>
                {
                    sc.SwaggerEndpoint(endpointUrl, endpointName);
                    sc.OAuthClientId(settings.OAuthClientId);
                    sc.OAuthAppName(settings.OAuthAppName);
                });

            return app;
        }
    }
}
