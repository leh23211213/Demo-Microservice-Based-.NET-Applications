using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace App.Services.ProductAPI.Models;

public class Size
{
    public int Id { get; set; }
    public string? RAM { get; set; }
    [NotMapped, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
