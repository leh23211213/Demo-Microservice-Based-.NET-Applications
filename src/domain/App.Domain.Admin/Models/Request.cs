
using static App.Domain.Admin.Utility.StaticDetail;
namespace App.Domain.Admin.Models;

public class Request
{
    public object? Data { get; set; }
    public string Url { get; set; } = null!;
    public ApiType ApiType { get; set; } = ApiType.GET;
    public ContentType ContentType { get; set; } = ContentType.Json;
}
