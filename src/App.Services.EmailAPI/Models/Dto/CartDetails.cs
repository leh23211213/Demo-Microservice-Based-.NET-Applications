namespace App.Services.EmailAPI.Models;
public class CartDetails
{
    public string? Id { get; set; }
    public string? CartHeaderId { get; set; }

    public virtual CartHeader? CartHeader { get; set; }
    public virtual Product? Product { get; set; }
    public string? ProductId { get; set; }
    public int Count { get; set; }
}