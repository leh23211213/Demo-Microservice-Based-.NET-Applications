
namespace App.Frontend.Utility;

public static class StaticDetail
{
    public static string CurrentAPIVersion = "v1";

    public static string AuthAPIBase { get; set; } = "";
    public static string ProductAPIBase { get; set; } = "";
    public static string ShoppingCartAPIBase { get; set; } = "";
    public static string OrderAPIBase { get; set; } = "";


    public static string AccessToken = "JWTToken";
    public static string RefreshToken = "RefreshToken";


    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";


    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }


    public enum ContentType
    {
        Json,
        MultipartFormData,
    }
}
