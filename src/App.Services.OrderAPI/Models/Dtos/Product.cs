using System.ComponentModel.DataAnnotations.Schema;
namespace App.Services.OrderAPI.Models;
[NotMapped]
public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public string? Description { get; set; } = null!;
    public virtual Size Size { get; set; } = null!;

    public virtual Color Color { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

}
