namespace App.Frontend.Models;
public class Order
{
    public int OrderId { get; set; }

    public string? CustomerName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public double TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetails>? OrderDetails { get; set; }
}