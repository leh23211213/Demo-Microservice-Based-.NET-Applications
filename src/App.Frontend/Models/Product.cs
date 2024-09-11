namespace App.Frontend.Models
{
    public class Product
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        

        public string Size { get; set; }
        public string Color { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
    }
}