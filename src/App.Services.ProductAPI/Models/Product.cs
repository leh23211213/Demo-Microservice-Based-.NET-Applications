namespace App.Services.ProductAPI.Models;
public partial class Product
{
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }

    // reference
    public int SizeId { get; set; }

    public int ColorId { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public Size Size { get; set; }
    public Color Color { get; set; }

    public Category Category { get; set; }

    public Brand Brand { get; set; }
}
