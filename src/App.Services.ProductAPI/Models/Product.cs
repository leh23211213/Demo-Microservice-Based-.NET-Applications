namespace App.Services.ProductAPI.Models;
public partial class Product
{
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public decimal Price { get; set; }

    // reference
    public int SizeId { get; set; }
    public Size Size { get; set; }

    public int ColorId { get; set; }
    public Color Color { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int BrandId { get; set; }
    public Brand Brand { get; set; }
}
