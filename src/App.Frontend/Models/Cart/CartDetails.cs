namespace App.Frontend.Models;
public class CartDetails
{
    public string? Id { get; set; }
    public int Count { get; set; } = 1;
    public string? CartHeaderId { get; set; }
    public string? ProductId { get; set; }
    public virtual CartHeader? CartHeader { get; set; }
    public virtual Product? Product { get; set; }
    public CartDetails()
    {
        Id = Guid.NewGuid().ToString();
    }
}