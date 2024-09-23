using System.ComponentModel.DataAnnotations.Schema;
using App.Services.ProductAPI.Extensions;

namespace App.Services.ProductAPI.Models;
public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    [NotMapped, SwaggerIgnore]
    public IFormFile? Image { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    [NotMapped, SwaggerIgnore]
    public virtual Size Size { get; set; }
    [NotMapped, SwaggerIgnore]
    public virtual Color Color { get; set; }
    [NotMapped, SwaggerIgnore]
    public virtual Category Category { get; set; }
    [NotMapped, SwaggerIgnore]
    public virtual Brand Brand { get; set; }
}
