namespace App.Frontend.Models
{
    public class Pagination
    {
        public Pagination()
        {
            Products = new List<Product>();
        }
        public List<Product>? Products { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
    }
}