
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.OrderAPI.Models;
[NotMapped]
public class CartHeader
{
    public string Id { get; set; } = null!;
    public string? UserId { get; set; } = null!;
    public string? CouponCode { get; set; } = "";
    public double Total { get; set; }
    public double? Discount { get; set; }
    public string? Name { get; set; } = "";
    public string? Phone { get; set; } = "";
    public string? Email { get; set; } = "";
}