using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Services.Identity.STS.IdentityServerConfig
{
    public static class ApiResourcesConfig
    {
        // ApiResources defines the apis in your system
        // http://docs.identityserver.io/en/release/reference/api_resource.html
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("resources", "Resources API"),
            };
        }
    }
}
