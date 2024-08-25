namespace App.Data.Models.ViewModels.Cart
{
    public class CartViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal TemporaryPrice { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public List<CartItemViewModel> CartItems { get; set; }
    }
}
