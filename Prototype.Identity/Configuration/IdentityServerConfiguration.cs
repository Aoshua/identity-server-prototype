using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Prototype.Identity.Configuration
{
    public class IdentityServerConfiguration
    {
        private static readonly string PrototypeApi = "Prototype-api";

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = PrototypeApi,
                    ClientName = "Prototype API description.",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false, // obsolete with PKCE
                    AllowOfflineAccess = true,
                    RedirectUris = new string[] { "https://localhost:8080/auth" },
                    PostLogoutRedirectUris = new string[] { "https://localhost:8080/logged-out" },
                    ClientSecrets = new Secret[] { new Secret("secret".Sha256()) },
                    AllowedScopes = new string[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        PrototypeApi
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(PrototypeApi, "Prototype API")
            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "aoshua",
                    Password = "123qwe",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Josh"),
                        new Claim(JwtClaimTypes.FamilyName, "Abbott"),
                        new Claim(JwtClaimTypes.Email, "josh@thereal.com")
                    }
                }
            };
    }
}
