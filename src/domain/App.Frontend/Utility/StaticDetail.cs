<<<<<<< HEAD
namespace App.Domain.Admin.Utility;
=======
namespace App.Frontend.Utility;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
public static class StaticDetail
{
    // secret for OIDC
    internal static string secret = "identityserverfundamentalsclients";
    //
<<<<<<< HEAD
    public static string? AuthAPIBase { get; set; }
    public static string? ProductAPIBase { get; set; }
    public static string? CouponAPIBase { get; set; }
    public static string? ShoppingCartAPIBase { get; set; }
    public static string? OrderAPIBase { get; set; }
<<<<<<<< HEAD:src/domain/App.Frontend/Utility/StaticDetail.cs
    public static string AccessToken = "AccessToken";
    public static string RefreshToken = "RefreshToken";
========
    // Tokens
    public static string AccessToken = "ac_tk";
    public static string RefreshToken = "rf_tk";
    // Roles
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Utility/StaticDetail.cs
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    // Products
=======
    internal static string? AuthAPIBase { get; set; }
    internal static string? ProductAPIBase { get; set; }
    internal static string? CouponAPIBase { get; set; }
    internal static string? ShoppingCartAPIBase { get; set; }
    internal static string? OrderAPIBase { get; set; }
    // Token
    public static string AccessToken = "ac_tk";
    public static string RefreshToken = "rf_tk";
    // Role
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    // Product
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
    public const string Size = "128GB";
    public const string Color = "Black";
    public const string Category = "Smartphone";
    public const string Brand = "Apple";
    // API
    public static string CurrentAPIVersion = "v1";
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE,
    }
    public enum ContentType
    {
        Json,
        MultipartFormData,
    }
    // Order
    public const string Status_Pending = "Pending";
    public const string Status_Approved = "Approved";
    public const string Status_ReadyForPickup = "ReadyForPickup";
    public const string Status_Completed = "Completed";
    public const string Status_Refunded = "Refunded";
    public const string Status_Cancelled = "Cancelled";

}
