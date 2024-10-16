namespace App.Services.ShoppingCartAPI.Models;
public class Cart
{
    public CartHeader CartHeader { get; set; } = null!;
    public IEnumerable<CartDetails>? CartDetails { get; set; }
}
