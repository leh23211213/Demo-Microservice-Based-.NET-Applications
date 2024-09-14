
using static App.Frontend.Utility.StaticDetail;
namespace App.Frontend.Models;

public class Request
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; } = null!;
    public object Data { get; set; } = null!; 
    public string AccessToken { get; set; } = null!;

    public ContentType ContentType { get; set; } = ContentType.Json;
}
