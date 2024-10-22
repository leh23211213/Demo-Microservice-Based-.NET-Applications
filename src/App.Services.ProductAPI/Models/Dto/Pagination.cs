namespace App.Services.ProductAPI.Models
{
    public class Pagination
    {
        public IEnumerable<PaginateProduct> Products { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
    }
}
