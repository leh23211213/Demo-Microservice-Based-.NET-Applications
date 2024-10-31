using Newtonsoft.Json;
namespace App.Services.ProductAPI.Models;
public class Size
{
    public int Id { get; set; }
    public string? RAM { get; set; }
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
