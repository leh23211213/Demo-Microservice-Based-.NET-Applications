using System.ComponentModel.DataAnnotations.Schema;

namespace App.Frontend.Models;
[NotMapped]
public class Cart
{
    public CartHeader CartHeader { get; set; }
    public IEnumerable<CartDetails>? CartDetails { get; set; }
}
