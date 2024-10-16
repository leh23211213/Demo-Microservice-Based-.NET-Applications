using App.Frontend.Utility;

namespace App.Frontend.Models
{
    public class Product
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public string? Description { get; set; }

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