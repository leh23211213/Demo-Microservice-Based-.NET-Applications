using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using App.Services.ShoppingCartAPI.Extensions;

namespace App.Services.ShoppingCartAPI.Models;

public class Size
{
    public int Id { get; set; }
    public string? RAM { get; set; }
    [NotMapped, SwaggerIgnore, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
