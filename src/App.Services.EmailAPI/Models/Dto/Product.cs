namespace App.Services.EmailAPI.Models;
public class Product
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public string? Description { get; set; }

    public IFormFile? Image { get; set; }
}
