using Swashbuckle.AspNetCore.Annotations;
namespace App.Services.ShoppingCartAPI.Models;
public class CartDetails
{
    public string? Id { get; set; }
    public string? CartHeaderId { get; set; }
    public string? ProductId { get; set; }
    public int Count { get; set; }

    // config FK
    [SwaggerIgnore]
    public virtual CartHeader? CartHeader { get; set; }
    [SwaggerIgnore]
    public virtual Product? Product { get; set; }
}