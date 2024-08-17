namespace App.Data.Models;

public partial class ProductDetail
{
    public string ProductDetailId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int ColorId { get; set; }

    public int SizeId { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
