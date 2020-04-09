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
                new Client
                {
                    ClientId = "web_base_administration",
                    ClientName = "Web Base Administration OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{clientsUrl["web_base_administration"]}/" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientsUrl["web_base_administration"]}/" },
                    AllowedCorsOrigins = { $"{clientsUrl["web_base_administration"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                },
                new Client
                {
                    ClientId = "localizationswaggerui",
                    ClientName = "Localization Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{clientsUrl["localization_api"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["localization_api"]}/swagger/" },
                    RequireConsent = false,
                    AllowedScopes =
                    {
                        "localization"
                    }
                }
            };
        }
    }
}
