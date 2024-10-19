namespace App.Services.OrderAPI.Models;
public class OrderDetails
{
    public string? Id { get; set; }
    public string? OrderHeaderId { get; set; }
    public int Count { get; set; } = 1;
    public decimal Price { get; set; } = 1;
    public string? ProductName { get; set; }
    public string? ProductId { get; set; }
    public Product? Product { get; set; }
}
