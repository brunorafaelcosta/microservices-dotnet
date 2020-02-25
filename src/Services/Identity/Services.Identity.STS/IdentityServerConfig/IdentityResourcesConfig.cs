using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Services.Identity.STS.IdentityServerConfig
{
    public static class IdentityResourcesConfig
    {
        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }
}
