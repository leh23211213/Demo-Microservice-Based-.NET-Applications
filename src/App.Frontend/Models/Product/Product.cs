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
        public IFormFile? Image { get; set; }

        public virtual Size? Size { get; set; }
        public virtual Color? Color { get; set; }

        public virtual Category? Category { get; set; }

        public virtual Brand? Brand { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}