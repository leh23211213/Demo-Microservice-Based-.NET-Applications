namespace App.Services.OrderAPI.Models;
public class OrderDetails
{
    public string? Id { get; set; }
    public string? OrderHeaderId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public string? ProductName { get; set; }
    public string? ProductId { get; set; }
    public Product? Product { get; set; }
}
