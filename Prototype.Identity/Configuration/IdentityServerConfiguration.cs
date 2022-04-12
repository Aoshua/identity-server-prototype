using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Prototype.Identity.Configuration
{
    public class IdentityServerConfiguration
    {
        private static readonly string ApiClient = "api-client";

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "vue-client",
                    ClientName = "Vue Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false, // obsolete with PKCE
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = false,
                    RedirectUris = new string[] { "http://localhost:8080/auth" },
                    PostLogoutRedirectUris = new string[] { "http://localhost:8080/logged-out" },
                    AllowedScopes = new string[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
                },
                new Client
                {
                    ClientId = "arcgis-client",
                    ClientName = "ArcGIS Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false, // obsolete with PKCE
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = false,
                    RedirectUris = new string[] { "https://elements.maps.arcgis.com/sharing/rest/oauth2/oidc/SAv68PjPSJvOW2HR/signin" },
                    PostLogoutRedirectUris = new string[] { "https://elements.maps.arcgis.com/sharing/rest/oauth2/oidc/SAv68PjPSJvOW2HR/signout" },
                    ClientSecrets = new Secret[] { new Secret("def2edf7-5d42-4edc-a84a-30136c340e13".Sha256()) }, // Not used, but ArcGIS asks for it
                    AllowedScopes = new string[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                //new ApiScope(ApiClient, "Prototype API")
            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
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
