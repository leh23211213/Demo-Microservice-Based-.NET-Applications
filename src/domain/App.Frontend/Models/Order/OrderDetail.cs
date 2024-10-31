using System.ComponentModel.DataAnnotations;
namespace App.Frontend.Models;
public class OrderDetails
{
    public string? Id { get; set; }
    public string? OrderHeaderId { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    [Range(1, 20, ErrorMessage = "Quantity must be at least 1 and maximum is 20")]
    public Product? product { get; set; }
}
