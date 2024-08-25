namespace App.Data.Models.ViewModels.Product
{
    public class ProductViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public int? Vote { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int? CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
