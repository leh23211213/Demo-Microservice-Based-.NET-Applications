using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace  template.Models;

public class Color
{
    public int Id { get; set; }
    public string? Name { get; set; }


    [NotMapped, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
