using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace App.Services.ProductAPI.Models;

public class Color
{
    public int Id { get; set; }
    public string? Name { get; set; }

    
    [NotMapped, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
