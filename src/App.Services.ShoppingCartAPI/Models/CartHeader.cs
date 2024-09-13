
namespace App.Services.ShoppingCartAPI.Models;
public class CartHeader
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public double CartTotal { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public CartHeader()
    {
        Id = Guid.NewGuid().ToString();
    }
}