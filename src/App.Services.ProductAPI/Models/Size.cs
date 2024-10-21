using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.ProductAPI.Models;

public class Size
{
    public int Id { get; set; }
    [SwaggerIgnore]
    public string? RAM { get; set; }
    [NotMapped, SwaggerIgnore]
    public ICollection<Product>? Products { get; set; }
}
