using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.ProductAPI.Models;
public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    [NotMapped]
    public IFormFile? Image { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    [NotMapped]
    public virtual Size Size { get; set; }
    [NotMapped]
    public virtual Color Color { get; set; }
    [NotMapped]
    public virtual Category Category { get; set; }
    [NotMapped]
    public virtual Brand Brand { get; set; }
}
