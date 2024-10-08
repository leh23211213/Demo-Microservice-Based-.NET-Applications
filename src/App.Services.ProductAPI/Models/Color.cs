using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using App.Services.ProductAPI.Extensions;
namespace App.Services.ProductAPI.Models;

public class Color
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    [NotMapped, SwaggerIgnore, JsonIgnore]
    public ICollection<Product> Products { get; set; } = null!;
}
