
using static App.Frontend.Utility.StaticDetail;
namespace App.Frontend.Models;

public class Request
{
    public object? Data { get; set; }
    public string Url { get; set; } = null!;
    public ApiType ApiType { get; set; } = ApiType.GET;
    public ContentType ContentType { get; set; } = ContentType.Json;
}
