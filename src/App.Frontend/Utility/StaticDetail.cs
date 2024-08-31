
namespace App.Frontend.Utility;

public class StaticDetail
{
    public static string AuthAPIBase{ get; set; }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
