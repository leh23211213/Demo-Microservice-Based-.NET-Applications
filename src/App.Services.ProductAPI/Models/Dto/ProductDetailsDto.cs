namespace App.Services.ProductAPI.Models;
public class ProductDetailsDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? Description { get; set; }
    public virtual Size? Size { get; set; }
    public virtual Color? Color { get; set; }
    public virtual Category? Category { get; set; }
    public virtual Brand? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
}
