using App.Frontend.Utility;

namespace App.Frontend.Models
{
    public class Product
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string? ImageUrl { get; set; } = null!;
        public string? ImageLocalPath { get; set; } = null!;
        public string? Description { get; set; } = null!;

        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; } = null!;

        public virtual Size Size { get; set; } = null!;

        public virtual Color Color { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        public virtual Brand Brand { get; set; } = null!;
    }
}