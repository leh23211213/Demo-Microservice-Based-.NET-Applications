
namespace App.Frontend.Utility;

public class StaticDetail
{
    public static string AuthAPIBase { get; set; }
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
