using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.OrderAPI.Models;
[NotMapped]
public class Cart
{
    public CartHeader CartHeader { get; set; } = null!;
    public IEnumerable<CartDetails>? CartDetails { get; set; } = null!;
}
