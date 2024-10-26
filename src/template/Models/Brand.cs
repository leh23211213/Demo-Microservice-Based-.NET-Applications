namespace template.Models;
public class Brand
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }


    public ICollection<Product>? Products { get; set; }
}
