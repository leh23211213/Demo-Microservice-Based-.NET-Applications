<<<<<<< HEAD
namespace App.Services.AuthAPI;
public static class StaticDetail
{
    public static string AccessToken = "AccessToken";
    public static string RefreshToken = "RefreshToken";
=======
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace App.Services.AuthAPI.Utility
{
    public static class StaticDetail
    {
        public static string AccessToken = "ac_tk";
        public static string RefreshToken = "rf_tk";
        public static string Admin = "ADMIN";
        public static string Customer = "CUSTOMER";

        #region OIDC configure

        /// <summary>
        /// [standard scopes] https://docs.duendesoftware.com/identityserver/v7/fundamentals/resources/identity/
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        /// <summary>
        /// [ApiScope] adocs.duendesoftware.com/identityserver/v7/fundamentals/resources/api_scopes/
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("user_scope", "user_scope Server"),
            new ApiScope("admin_scope", "admin Server"),
            };
        /// <summary>
        /// https://docs.duendesoftware.com/identityserver/v7/fundamentals/clients/
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "user_scope",
                    ClientName = "user client",
                    ClientSecrets = { new Secret(secret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowedScopes = {
                            "user_scope", "admin_scope","magic","read","write","delete",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            JwtClaimTypes.Role
                    },
                    RedirectUris = RedirectUris,
                    PostLogoutRedirectUris = PostLogoutRedirectUris,
                },
            };
        private static string secret = "identityserverfundamentalsclients";
        // TODO : production ?
        private static List<string> RedirectUris = new()
        {
           "https://localhost:7000/Account/Authentication/signin-oidc",
        };
        private static List<string> PostLogoutRedirectUris = new()
        {
            "https://localhost:7000/Account/Authentication/signout-callback-oidc",
        };

        #endregion
    }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
}
