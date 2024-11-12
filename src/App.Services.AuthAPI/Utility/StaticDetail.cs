using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace App.Services.AuthAPI.Utility;
public static class StaticDetail
{
    public static string AccessToken = "ac_tk";
    public static string RefreshToken = "rf_tk";
    public static string Admin = "ADMIN";
    public static string Customer = "CUSTOMER";
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
        new ApiScope("user_scope", "user server"),
        new ApiScope("admin_scope", "admin Server"),
    };

    private static string secret = "identityserverfundamentalsclients";
    // TODO : production ?
    private static string RedirectUris = "/signin-oidc";
    private static string PostLogoutRedirectUris = "/signout-oidc";
    /// <summary>
    /// https://docs.duendesoftware.com/identityserver/v7/fundamentals/clients/
    /// </summary>
    public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "admin_scope",
            ClientName = "admin client",
            ClientSecrets = { new Secret(secret.Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            AllowedScopes = {
               "user_scope", "admin_scope",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                JwtClaimTypes.Role
            },
            RedirectUris={ RedirectUris },
            PostLogoutRedirectUris = { PostLogoutRedirectUris },
        },
        new Client
        {
            ClientId = "user_scope",
            ClientName = "user client",
            ClientSecrets = { new Secret(secret.Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            AllowedScopes = {
                "user_scope", "admin_scope",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                JwtClaimTypes.Role
            },
            RedirectUris={ RedirectUris },
            PostLogoutRedirectUris = { PostLogoutRedirectUris },
        },
    };
}
