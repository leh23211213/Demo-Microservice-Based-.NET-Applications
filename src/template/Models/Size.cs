namespace template.Models;
public class Size
{
    public int Id { get; set; }
    public string? RAM { get; set; }


    public ICollection<Product>? Products { get; set; }
}
