namespace App.Services.ProductAPI.Models;

public partial class Size
{
    public int SizeId { get; set; }
    public string RAM { get; set; } = null!;
    public ICollection<Product> Products { get; set; }
}
