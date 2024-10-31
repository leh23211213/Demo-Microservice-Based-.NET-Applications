namespace App.Domain.Admin.Utility;
public static class StaticDetail
{
    public static string CurrentAPIVersion = "v1";
    public static string? AuthAPIBase { get; set; }
    public static string? ProductAPIBase { get; set; }
    public static string? CouponAPIBase { get; set; }
    public static string? ShoppingCartAPIBase { get; set; }
    public static string? OrderAPIBase { get; set; }
    public static string AccessToken = "AccessToken";
    public static string RefreshToken = "RefreshToken";
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public const string Size = "128GB";
    public const string Color = "Black";
    public const string Category = "Smartphone";
    public const string Brand = "Apple";
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE,
    }
    public const string Status_Pending = "Pending";
    public const string Status_Approved = "Approved";
    public const string Status_ReadyForPickup = "ReadyForPickup";
    public const string Status_Completed = "Completed";
    public const string Status_Refunded = "Refunded";
    public const string Status_Cancelled = "Cancelled";
    public enum ContentType
    {
        Json,
        MultipartFormData,
    }
}