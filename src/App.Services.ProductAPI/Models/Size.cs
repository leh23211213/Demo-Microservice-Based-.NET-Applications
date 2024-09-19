namespace App.Services.ProductAPI.Models;

public class Size
{
    public int Id { get; set; }
    public string RAM { get; set; } = null!;
    public ICollection<Product> Products { get; set; }
}
