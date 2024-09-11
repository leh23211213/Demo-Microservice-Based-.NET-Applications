using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.ShoppingCartAPI.Models;
[NotMapped]
public class Cart
{
    public CartHeader cartHeader { get; set; }
    public IEnumerable<CartDetails>? cartDetails { get; set; }
}
