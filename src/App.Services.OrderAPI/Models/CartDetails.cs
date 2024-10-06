using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.OrderAPI.Models;
[NotMapped]
public class CartDetails
{
    public string Id { get; set; } = "";
    public string CartHeaderId { get; set; } = null!;
    public virtual CartHeader CartHeader { get; set; } = null!;
    public string ProductId { get; set; } = null!;
    [NotMapped]
    public virtual Product Product { get; set; } = null!;
    public int Count { get; set; }
}