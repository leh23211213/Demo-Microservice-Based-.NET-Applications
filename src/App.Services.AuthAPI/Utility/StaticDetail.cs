using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace App.Services.AuthAPI;
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
    /// [ApiScope] fadocs.duendesoftware.com/identityserver/v7/fundamentals/resources/api_scopes/
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
    new List<ApiScope>
    {
        new ApiScope("levanhiep409159", "levanhiep409159 Server"),
        new ApiScope(name: "productapi", displayName: "Product Server"),
        new ApiScope(name: "cartapi", displayName: "Cart Server"),
        new ApiScope(name: "orderapi", displayName: "Order Server"),
    };

    private static string secret = "identityserverfundamentalsclients";
    // TODO : production ?
    private static string RedirectUris = "https://localhost:7000/signin-oidc";
    private static string PostLogoutRedirectUris = "https://localhost:7000/signout-callback-oidc";
    /// <summary>
    /// https://docs.duendesoftware.com/identityserver/v7/fundamentals/clients/
    /// </summary>
    public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "levanhiep409159",
            ClientSecrets = { new Secret(secret.Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            AllowedScopes = {
                "levanhiep409159",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                JwtClaimTypes.Role
            },
            RedirectUris={ RedirectUris },
            PostLogoutRedirectUris = { PostLogoutRedirectUris },
        },
        // new Client
        // {
        //     ClientId = "productapi",
        //     ClientSecrets = { new Secret(secret.Sha256()) },
        //     AllowedGrantTypes = GrantTypes.Code,
        //     AllowedScopes = {
        //         "productapi",
        //         IdentityServerConstants.StandardScopes.OpenId,
        //         IdentityServerConstants.StandardScopes.Profile,
        //         IdentityServerConstants.StandardScopes.Email,
        //         JwtClaimTypes.Role
        //     },
        //      RedirectUris={ RedirectUris },
        //     PostLogoutRedirectUris={PostLogoutRedirectUris},
        // },
        // new Client
        // {
        //     ClientId = "cartapi",
        //     ClientSecrets = { new Secret(secret.Sha256()) },
        //     AllowedGrantTypes = GrantTypes.Code,
        //     AllowedScopes = { "cartapi",
        //         IdentityServerConstants.StandardScopes.OpenId,
        //         IdentityServerConstants.StandardScopes.Profile,
        //         IdentityServerConstants.StandardScopes.Email,
        //         JwtClaimTypes.Role
        //     },
        //     RedirectUris={ RedirectUris },
        //     PostLogoutRedirectUris={PostLogoutRedirectUris},
        // },
        // new Client
        // {
        //     ClientId = "orderapi",
        //     ClientSecrets = { new Secret(secret.Sha256()) },
        //     AllowedGrantTypes = GrantTypes.Code,
        //     AllowedScopes = { "orderapi",
        //         IdentityServerConstants.StandardScopes.OpenId,
        //         IdentityServerConstants.StandardScopes.Profile,
        //         IdentityServerConstants.StandardScopes.Email,
        //         JwtClaimTypes.Role
        //     },
        //     RedirectUris={ RedirectUris },
        //     PostLogoutRedirectUris={PostLogoutRedirectUris},
        // },
    };
}
