using System.Collections.Generic;

namespace Services.Identity.STS
{
    public class Settings
    {
        public Dictionary<string, string> InternalEndpoints { get; set; }

        public string ExternalEndpoint { get; set; }
    }
}
