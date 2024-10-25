using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
namespace App.Services.ProductAPI.Models;
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
    [SwaggerIgnore]
    public string? ImageUrl { get; set; }
    [SwaggerIgnore]
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