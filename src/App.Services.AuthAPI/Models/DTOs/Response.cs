
namespace App.Services.AuthAPI.Models.DTOs
{
    public class Response
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}