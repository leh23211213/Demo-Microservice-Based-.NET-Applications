using App.Data.Enums;

namespace App.Data.Models;
public class Order
{
    public int OrderId { get; set; }

    public string UserId { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string CustomerName { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}