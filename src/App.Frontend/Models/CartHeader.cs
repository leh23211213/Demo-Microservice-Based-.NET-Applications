
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Frontend.Models;
public class CartHeader
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string? CouponCode { get; set; }
    [NotMapped]
    public double? Discount { get; set; }

    public CartHeader()
    {
        Id = Guid.NewGuid().ToString();
    }
}