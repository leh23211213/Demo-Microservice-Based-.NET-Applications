using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace App.Services.ProductAPI.Models;

public class Color
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [NotMapped, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
