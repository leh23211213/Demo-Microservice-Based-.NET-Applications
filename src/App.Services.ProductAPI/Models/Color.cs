namespace App.Services.ProductAPI.Models;

public partial class Color
{
    public int ColorId { get; set; }
    public string ColorName { get; set; } = null!;
    public ICollection<Product> Products { get; set; }
}
