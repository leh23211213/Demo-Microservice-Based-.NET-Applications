
using System.ComponentModel.DataAnnotations.Schema;
using App.Services.ShoppingCartAPI.Extensions;

namespace App.Services.ShoppingCartAPI.Models;
public class CartHeader
{
    public string? Id { get; set; }
    public string UserId { get; set; }

    [NotMapped]
    public double Total { get; set; }
    [NotMapped]
    public double Discount { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    [NotMapped, SwaggerIgnore]
    public virtual ICollection<CartDetails>? CartDetails { get; set; }
}