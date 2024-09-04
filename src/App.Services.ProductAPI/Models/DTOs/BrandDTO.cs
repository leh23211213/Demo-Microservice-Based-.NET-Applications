

namespace App.Services.ProductAPI.Models.DTOs
{
    public class BrandDTO
    {
        public int BrandId { get; set; }

        public string BrandName { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}