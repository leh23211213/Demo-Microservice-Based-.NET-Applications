namespace App.Frontend.Models
{
    public class Product
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public string? ImageLocalPath { get; set; } = null!;

        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; } = null!;

        [Range(1, 10)]
        public int Count { get; set; } = 1;
        
        public double Price { get; set; } = 0;

        public string? Size { get; set; } = null!;
        public string? Color { get; set; } = null!;
        public string? Category { get; set; } = null!;
        public string? Brand { get; set; } = null!;
    }
}