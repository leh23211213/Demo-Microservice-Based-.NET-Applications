namespace App.Services.OrderAPI.Models;
public class OrderHeader
{
    public int Id { get; set; }
    public string? UserId { get; set; } = null!;
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double OrderTotal { get; set; }
    public DateTime OrderTime { get; set; } = DateTime.Now;
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Status { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? StripeSessionId { get; set; }
    public IEnumerable<OrderDetail> OrderDetails { get; set; }
}