using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ShoppingCartAPI.Models;
[NotMapped]
public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [NotMapped, SwaggerIgnore, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
