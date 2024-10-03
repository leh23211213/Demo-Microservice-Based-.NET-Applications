using System.Net;
using System.Text.Json.Serialization;

namespace App.Services.ProductAPI.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("result")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object? Result { get; set; }

        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Extra information if any (e.g. the detailed error message).
        /// </summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; } = "";
    }
}