using Transversal.Web.API.Swagger;

namespace Transversal.Web.API
{
    public abstract class APIBootstrapperSettingsBase : WebBootstrapperSettingsBase
    {
        public bool WriteIndentedJson { get; set; }

        #region Authentication

        public class OAuthSettings
        {
            public string Authority { get; set; }
            public string Audience { get; set; }
            public bool RequireHttpsMetadata { get; set; }
        }

        public OAuthSettings Authentication { get; set; }

        #endregion

        public virtual SwaggerSettings Swagger { get; set; }

        protected override void Defaults()
        {
            base.Defaults();

            WriteIndentedJson = true;

            Authentication = new OAuthSettings
            {
                RequireHttpsMetadata = true
            };

            Swagger = new SwaggerSettings
            {
                IsEnabled = false
            };
        }
    }
}
