using App.Data.Enums;

namespace App.Data.Models;

public class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public OrderDetailStatus Status { get; set; }


}
