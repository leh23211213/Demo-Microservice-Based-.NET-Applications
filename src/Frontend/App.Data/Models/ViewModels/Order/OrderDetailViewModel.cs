using System.ComponentModel.DataAnnotations;
using App.Data.Enums;
namespace App.Data.Models.ViewModels.Order;
public class OrderDetailsViewModel
{
    public int OrderDetailId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public decimal Price { get; set; }
    public OrderDetailStatus Status { get; set; }
}
