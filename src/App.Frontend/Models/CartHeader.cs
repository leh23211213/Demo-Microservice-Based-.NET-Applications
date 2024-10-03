using System.ComponentModel.DataAnnotations;
namespace App.Frontend.Models;
public class CartHeader
{
    public string Id { get; set; } = null!;
    public string? UserId { get; set; } = null!;
    public string? CouponCode { get; set; }

    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Phone { get; set; }
    [Required]
    public string? Email { get; set; }
    public double? Discount { get; set; }
    public CartHeader()
    {
        Id = Guid.NewGuid().ToString();
    }
}