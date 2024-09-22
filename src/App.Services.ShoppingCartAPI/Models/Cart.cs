using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.ShoppingCartAPI.Models;
[NotMapped]
public class Cart
{
    public CartHeader CartHeader { get; set; }
    public IEnumerable<CartDetails>? CartDetails { get; set; }
}
