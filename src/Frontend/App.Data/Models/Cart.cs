namespace App.Data.Models;
public partial class Cart
{
    public string CartId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;

    public Cart()
    {
        CartId = Guid.NewGuid().ToString(); // Tạo GUID mới cho CartId khi khởi tạo Cart
    }
}
