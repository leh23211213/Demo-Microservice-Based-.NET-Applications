using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.OrderAPI.Models;
[NotMapped]
public class Response
{
    public object? Result { get; set; } = null!;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "";
}
