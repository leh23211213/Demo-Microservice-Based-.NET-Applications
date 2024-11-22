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
            new ApiScope("api2_scope", "api2_scope Server"),
            new ApiScope("api1_scope", "api1_scope Server"),
            new ApiScope("user", "api1_scope Server"),
            };
        /// <summary>
        /// https://docs.duendesoftware.com/identityserver/v7/fundamentals/clients/
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("user")
            {
                Scopes = new List<string> {"user.read", "user.update"},
                ApiSecrets = new List<Secret> {new Secret(secret.Sha256())},
                UserClaims = new List<string> {"name","role"}
            }
        };

        public static IEnumerable<Client> Clients =>

            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret(secret.Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "user",
                    ClientName = "user server",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    // AC TOKEN
                    AllowOfflineAccess = true, // Enable refresh tokens
                    AccessTokenLifetime = 3600, // Lifetime of access token (1 hour)  // Thời gian sống của Access Token (tính bằng giây)
                     // RF TOKEN
                    RefreshTokenUsage = TokenUsage.ReUse, // Re-use or One-Time Use RefreshTokenUsage = TokenUsage.OneTimeOnly, Xoá Refresh Token cũ sau khi sử dụng
                    RefreshTokenExpiration = TokenExpiration.Sliding, // Tự động kéo dài thời gian sống RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 2592000, // 30 days
                    SlidingRefreshTokenLifetime = 86400, // Thời gian kéo dài (tính bằng giây)

                   // RequirePkce = true,
                    AllowedScopes = AllowedScopes,
                    RedirectUris = RedirectUris,
                    PostLogoutRedirectUris = PostLogoutRedirectUris,
                },
            };
        private static string secret = "identityserverfundamentalsclients";
        // TODO : production ?
        private static List<string> AllowedScopes = new()
        {
            "user",
            "api1_scope",
            "api2_scope",
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.Email,
            JwtClaimTypes.Role
        };
        private static List<string> RedirectUris = new()
        {
            "https://localhost:7000/signin-oidc",
            "https://localhost:7000/Authentication/signin-oidc",
            "https://localhost:7000/Authentication/Login",
        };
        private static List<string> PostLogoutRedirectUris = new()
        {
            "https://localhost:7000/signout-callback-oidc",
            "https://localhost:7000/Authentication/signout",
            "https://localhost:7000/Authentication/Logout",
        };

        #endregion
    }
}