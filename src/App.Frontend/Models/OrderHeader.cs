namespace App.Frontend.Models;
public class Order
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? Address { get; set; } = null!;

    public string? City { get; set; } = null!;

    public string? Country { get; set; } = null!;

    public decimal TotalPrice { get; set; } = 0;
    public DateTime OrderDate { get; set; }
    public List<OrderDetails> OrderDetails { get; set; } = null!;
}