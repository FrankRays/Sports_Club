using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sport.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Jonas Jonaitis",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Jonas"),
                        new Claim("family_name", "Jonaitis"),
                        new Claim("role", "Trainer")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "123456",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Petras"),
                        new Claim("family_name", "Petraitis"),
                        new Claim("role", "Client")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new List<string>() {"role"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("sportapi", "Sport API",
                new List<string>() {"role"})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Gym Information System",
                    ClientId = "sportclient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44302/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "sportapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44302/signout-callback-oidc"
                    }
                    //AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
