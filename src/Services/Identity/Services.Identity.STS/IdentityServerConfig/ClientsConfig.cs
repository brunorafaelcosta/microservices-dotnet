using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Services.Identity.STS.IdentityServerConfig
{
    public static class ClientsConfig
    {
        // Clients that want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
            };
        }
    }
}
