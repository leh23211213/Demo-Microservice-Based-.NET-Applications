using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
namespace App.Services.AuthAPI.Models;
[NotMapped]
public class Response
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public object? Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "";
}