using Swashbuckle.AspNetCore.Annotations;

namespace App.Services.ProductAPI.Models;
public class ProductDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? Description { get; set; }
    public virtual Size? Size { get; set; }
    public virtual Color? Color { get; set; }
    public virtual Category? Category { get; set; }
    public virtual Brand? Brand { get; set; }

    [SwaggerIgnore]
    public string? ImageUrl { get; set; }
    [SwaggerIgnore]
    public string? ImageLocalPath { get; set; }
}
