namespace App.Services.ProductAPI.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? ImageUrl { get; set; }
    public ICollection<Product> Products { get; set; }
}
