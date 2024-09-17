using System.Net;
namespace App.Frontend.Models;

public class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public object? Result { get; set; } = null!;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "";
    public List<string> ErrorMessages { get; set; }
}
