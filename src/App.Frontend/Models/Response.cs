
namespace App.Frontend.Models;

public class Response
{
    public object? Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "";
}
