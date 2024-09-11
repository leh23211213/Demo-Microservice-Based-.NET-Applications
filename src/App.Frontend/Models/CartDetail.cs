namespace App.Frontend.Models;
public class CartItemDetails
{
    public int CartItemId { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TemporaryPrice { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
}
