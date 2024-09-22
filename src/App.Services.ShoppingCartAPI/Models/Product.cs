using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.ShoppingCartAPI.Models;
[NotMapped]
public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public IFormFile? Image { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
}
