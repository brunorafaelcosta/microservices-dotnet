using System.Collections.Generic;
using Transversal.Web;

namespace Services.Identity.STS
{
    public class Settings : WebBootstrapperSettingsBase
    {
        public Dictionary<string, string> InternalEndpoints { get; set; }

        public string ExternalEndpoint { get; set; }
    }
}
