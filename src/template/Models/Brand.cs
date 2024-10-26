using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace  template.Models;
public class Brand
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }


    [NotMapped, JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
