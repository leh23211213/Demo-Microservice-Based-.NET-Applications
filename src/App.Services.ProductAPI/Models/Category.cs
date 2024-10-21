using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ProductAPI.Models;

public class Category
{
    public int Id { get; set; }
    [SwaggerIgnore]
    public string? Name { get; set; }
    [NotMapped, SwaggerIgnore]
    public ICollection<Product>? Products { get; set; }
}
