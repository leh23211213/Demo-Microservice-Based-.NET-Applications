using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ProductAPI.Models;

public class Product
{
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Name must not contain special characters.")]
    public string Name { get; set; } = "";

    private double _price = 0;
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

    [NotMapped]
    public IFormFile? Image { get; set; }

    [NotMapped]
    public virtual Size? Size { get; set; }
    [NotMapped]
    public virtual Color? Color { get; set; }
    [NotMapped]
    public virtual Category? Category { get; set; }
    [NotMapped]
    public virtual Brand? Brand { get; set; }
}