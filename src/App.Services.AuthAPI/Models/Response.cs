using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
namespace App.Services.AuthAPI.Models;
[NotMapped]
public class Response
{
    public Response()
    {
        ErrorMessages = new List<string>();
    }
    public HttpStatusCode StatusCode { get; set; }
    public object? Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string> ErrorMessages { get; set; }
}