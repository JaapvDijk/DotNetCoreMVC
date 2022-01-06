using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address =
                    new
                    {
                        street_address = "Straat",
                        locality = "Heidelberg",
                        postal_code = 2954,
                        country = "Nederland"
                    };

                return new List<TestUser> {
                    new TestUser {
                        SubjectId = "818727",
                        Username = "john",
                        Password = "john",
                        Claims =
                            {
                                new Claim(JwtClaimTypes.Name, "John Smith"),
                                new Claim(JwtClaimTypes.GivenName, "John"),
                                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                                new Claim(JwtClaimTypes.EmailVerified, "true",
                                    ClaimValueTypes.Boolean),
                                new Claim(JwtClaimTypes.Role, "admin"),
                                new Claim(JwtClaimTypes.Address,
                                    JsonSerializer.Serialize(address),
                                    IdentityServerConstants
                                        .ClaimValueTypes
                                        .Json)
                            }
                    },
                    
                    new TestUser {
                        SubjectId = "88421113",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                            {
                                new Claim(JwtClaimTypes.Name, "Bob Smith"),
                                new Claim(JwtClaimTypes.GivenName, "Bob"),
                                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim(JwtClaimTypes.Role, "user"),
                                new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                                new Claim(JwtClaimTypes.Address,
                                    JsonSerializer.Serialize(address),
                                    IdentityServerConstants
                                        .ClaimValueTypes
                                        .Json)
                            }
                    }
                };
            }
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("weatherapi.read"),
                new ApiScope("weatherapi.write")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("an.api")
                {
                    Scopes =
                        new List<string> {
                            "weatherapi.read",
                            "weatherapi.write"
                        },
                    ApiSecrets =
                        new List<Secret> { new Secret("ScopeSecret".Sha256()) },
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                // api client credentials flow client
                new Client {
                    ClientId = "an.api",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =  { new Secret("SuperSecretPassword".Sha256()) },
                    AllowedScopes = { "weatherapi.read", "weatherapi.write" }
                },
                // interactive client using code flow + pkce
                new Client {
                    ClientId = "mvc.client",
                    ClientSecrets =
                        { new Secret("SuperSecretPassword".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:5000/DotNetLearning/signin" }, //http://localhost:5000/signin dev
                    FrontChannelLogoutUri = "http://localhost:5000/DotNetLearning/signout",
                    PostLogoutRedirectUris = { "http://localhost:5000/DotNetLearning/signout-callback" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "weatherapi.read" },
                    RequirePkce = true,
                    RequireConsent = false,
                    AllowPlainTextPkce = false
                }
            };
    }
}
