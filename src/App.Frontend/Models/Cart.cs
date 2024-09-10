namespace App.Frontend.Models;

public class Cart
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public decimal TemporaryPrice { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
    public List<CartItemDetails> CartItems { get; set; }
}
