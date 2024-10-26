namespace template.Models;

public class Color
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Product>? Products { get; set; }
}
