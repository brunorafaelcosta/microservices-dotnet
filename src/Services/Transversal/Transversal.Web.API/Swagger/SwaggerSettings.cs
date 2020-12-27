using System.Collections.Generic;

namespace Transversal.Web.API.Swagger
{
    public class SwaggerSettings
    {
        public bool IsEnabled { get; set; }

        public string EndpointUrl { get; set; }
        public string EndpointName { get; set; }

        public string OAuthClientId { get; set; }
        public string OAuthAppName { get; set; }
        public string OAuthAuthorizationUrl { get; set; }
        public string OAuthTokenUrl { get; set; }
        public IDictionary<string, string> OAuthScopes { get; set; }

        public string DocName { get; set; }
        public string DocTitle { get; set; }
        public string DocVersion { get; set; }
        public string DocDescription { get; set; }
    }
}
