
namespace App.Frontend.Models;

public class Response
{
    public object? Result { get; set; } = null!;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "";
}
