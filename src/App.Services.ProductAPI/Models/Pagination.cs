using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.ProductAPI.Models
{
    [NotMapped]
    public class Pagination
    {
        public IEnumerable<Product> Products { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
    }
}
