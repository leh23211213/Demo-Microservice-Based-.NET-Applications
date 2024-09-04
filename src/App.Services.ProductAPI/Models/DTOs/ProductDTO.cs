

namespace App.Services.ProductAPI.Models.DTOs
{
    public class ProductDTO
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
    }
}