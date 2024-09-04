namespace App.Services.ProductAPI.Models;

public partial class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public ICollection<Product> Products { get; set; }
}
