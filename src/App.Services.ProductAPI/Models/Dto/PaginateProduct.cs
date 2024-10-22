using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ProductAPI.Models
{
    public class PaginateProduct
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string? Description { get; set; }
        [SwaggerIgnore]
        public string? ImageUrl { get; set; }
        [SwaggerIgnore]
        public string? ImageLocalPath { get; set; }
    }
}