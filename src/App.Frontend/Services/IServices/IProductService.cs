using App.Frontend.Models;
namespace App.Frontend.Services.IServices
{
    public interface IProductService
    {
        /// <summary>
        /// Get all product
        /// </summary>
        Task<Response?> GetAllProduct();

        /// <summary>
        /// Get product by Id
        /// </summary>
        Task<Response?> Get(string id);

        /// <summary>
        /// $"?search={search}+currentPage={currentPage}"
        /// </summary>
        Task<Response?> Get(string search, int currentPage);

        Task<Response?> CreateAsync(Product product);
        Task<Response?> UpdateAsync(Product product);
        Task<Response?> DeleteAsync(string id);
    }
}