namespace App.Frontend.Models
{
    public class PaginationDTO
    {
        public IEnumerable<Product> Products { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
    }
}