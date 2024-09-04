
namespace App.Services.ProductAPI.Models.DTOs
{
    public class Response
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}