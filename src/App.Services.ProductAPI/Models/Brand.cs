using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ProductAPI.Models;

public class Brand
{
    public int Id { get; set; }
    [SwaggerIgnore]
    public string? Name { get; set; }
    [SwaggerIgnore]
    public string? ImageUrl { get; set; }
    [NotMapped, SwaggerIgnore]
    public ICollection<Product>? Products { get; set; }
}
