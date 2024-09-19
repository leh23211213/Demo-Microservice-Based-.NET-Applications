
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace App.Services.ProductAPI.Models.DTOs
{
    [NotMapped]
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}