using System.ComponentModel.DataAnnotations;
using App.Frontend.Utility;

namespace App.Frontend.Models
{
    public class Product
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; } = null!;

        [Range(1, 10)]
        public int Count { get; set; } = 1;
        public double Price { get; set; } = 0;

        public string? ImageUrl { get; set; } = null!;
        public string? ImageLocalPath { get; set; } = null!;
        public string Description { get; set; } = null!;

        public Size Size { get; set; } = null!;

        public Color Color { get; set; } = null!;

        public Category Category { get; set; } = null!;

        public Brand Brand { get; set; } = null!;
    }
}