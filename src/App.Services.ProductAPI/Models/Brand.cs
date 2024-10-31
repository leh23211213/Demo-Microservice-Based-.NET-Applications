using Newtonsoft.Json;
namespace App.Services.ProductAPI.Models;
public class Brand
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
