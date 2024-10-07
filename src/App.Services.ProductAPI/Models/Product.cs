using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using App.Services.ProductAPI.Extensions;

namespace App.Services.ProductAPI.Models;
public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? ImageUrl { get; set; } = null!;
    public string? ImageLocalPath { get; set; } = null!;
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    [NotMapped, SwaggerIgnore, JsonIgnore]
    public IFormFile? Image { get; set; } = null!;

    [NotMapped, SwaggerIgnore, JsonIgnore]
    public virtual Size Size { get; set; } = null!;

    [NotMapped, SwaggerIgnore, JsonIgnore]
    public virtual Color Color { get; set; } = null!;

    [NotMapped, SwaggerIgnore, JsonIgnore]
    public virtual Category Category { get; set; } = null!;

    [NotMapped, SwaggerIgnore, JsonIgnore]
    public virtual Brand Brand { get; set; } = null!;
}
