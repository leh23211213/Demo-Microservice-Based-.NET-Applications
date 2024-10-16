
namespace App.Services.OrderAPI.Models;

public class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }
}