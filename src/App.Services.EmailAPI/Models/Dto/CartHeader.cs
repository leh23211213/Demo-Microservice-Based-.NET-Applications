namespace App.Services.EmailAPI.Models;
public class CartHeader
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public double Total { get; set; }

    public double Discount { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public virtual ICollection<CartDetails>? CartDetails { get; set; }
}