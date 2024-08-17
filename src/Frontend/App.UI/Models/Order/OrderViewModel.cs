using System.ComponentModel.DataAnnotations;
using App.Data.Enums;

namespace App.UI.Models.Order;
public class OrderViewModel
{
    public int OrderId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string CustomerName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetailsViewModel> OrderDetails { get; set; }
}