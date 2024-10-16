namespace App.Services.OrderAPI.Models;
public class OrderDetail
{
    public int Id { get; set; }
    public int OrderHeaderId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public string? ProductName { get; set; }
    public string? ProductId { get; set; }
    public Product? product { get; set; }
}
