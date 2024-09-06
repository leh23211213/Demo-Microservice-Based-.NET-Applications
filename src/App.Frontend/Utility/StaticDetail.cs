
namespace App.Frontend.Utility;

public class StaticDetail
{
    public static string AuthAPIBase { get; set; }
    public static string ProductAPIBase { get; set; }
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public const string TokenCookie = "JWTToken";
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
