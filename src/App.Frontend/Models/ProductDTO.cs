

namespace App.Frontend.Models
{
    public class ProductDTO
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}