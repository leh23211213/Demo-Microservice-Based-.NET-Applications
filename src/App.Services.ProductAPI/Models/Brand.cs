using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ProductAPI.Models;

public class Brand
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    [NotMapped, JsonIgnore, SwaggerIgnore]
    public ICollection<Product>? Products { get; set; }
}
