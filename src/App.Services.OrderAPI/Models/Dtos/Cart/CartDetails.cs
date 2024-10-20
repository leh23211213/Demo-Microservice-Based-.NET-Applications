namespace App.Services.OrderAPI.Models;
public class CartDetails
{
    public string? Id { get; set; }
    public string? CartHeaderId { get; set; }
    public virtual CartHeader? CartHeader { get; set; }
    public string? ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public int Count { get; set; }
}