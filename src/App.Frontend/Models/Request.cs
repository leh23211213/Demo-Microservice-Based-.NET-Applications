
using static App.Frontend.Utility.StaticDetail;

namespace App.Frontend.Models;

public class Request
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }

    public ContentType ContentType { get; set; } = ContentType.Json;
}
