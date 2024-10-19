
namespace App.Services.OrderAPI.Models;
public class CartHeader
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Total { get; set; }
    public double Discount { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}