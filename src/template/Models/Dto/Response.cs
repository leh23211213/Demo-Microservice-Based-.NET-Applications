using System.Net;
namespace template.Models;
public class Response
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public object? Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    /// <summary>
    /// Extra information if any (e.g. the detailed error message).
    /// </summary>
    public string? Message { get; set; } = "";
}
