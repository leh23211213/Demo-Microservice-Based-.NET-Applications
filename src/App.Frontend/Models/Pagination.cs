namespace App.Frontend.Models
{
    public class Pagination
    {
        public Pagination()
        {
            Products = new List<Product>();
        }
        public List<Product>? Products { get; set; } = null!;
        public int totalPages { get; set; } = 0;
        public int currentPage { get; set; } = 1;
    }
}