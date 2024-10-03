namespace App.Frontend.Models;

public class Color
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Product> Products { get; set; }
}
