using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.OrderAPI.Models;
[NotMapped]
public class Response
{
    public object? Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "";
}
