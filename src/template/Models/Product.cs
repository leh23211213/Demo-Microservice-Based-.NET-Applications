namespace template.Models;
public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    private double _price;
    public double Price
    {
        get => _price;
        set
        {
            if (value > 0)
            {
                _price = value;
            }
            else
            {
                throw new ArgumentException("Price must be greater than 0.");
            }
        }
    }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }

    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    public IFormFile? Image { get; set; }

    public virtual Size? Size { get; set; }
    public virtual Color? Color { get; set; }
    public virtual Category? Category { get; set; }
    public virtual Brand? Brand { get; set; }
}