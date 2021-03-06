using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Transversal.Web.API.Swagger.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        readonly string _scope;

        public AuthorizeCheckOperationFilter()
        {
            _scope = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            operation.Responses.TryAdd("401", new OpenApiResponse() { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse() { Description = "Forbidden" });

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ oAuthScheme ] = new [] { _scope }
                }
            };
        }
    }
}
