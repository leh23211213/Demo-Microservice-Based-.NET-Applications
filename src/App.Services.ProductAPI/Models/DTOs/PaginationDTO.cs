namespace App.Services.ProductAPI.Models.DTOs
{
    public class PaginationDTO
    {
        public IEnumerable<Product> products { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
    }
}
