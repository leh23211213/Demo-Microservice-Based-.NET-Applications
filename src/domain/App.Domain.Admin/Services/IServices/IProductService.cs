using App.Domain.Admin.Models;
namespace App.Domain.Admin.Services.IServices
{
    public interface IProductService
    {
        /// <summary>
        /// Get
        /// </summary>
        Task<Response?> Get();

        /// <summary>
        /// Get product by Id
        /// </summary>
        Task<Response?> Get(string id);

        /// <summary>
        ///  Pagination/$"?pageSize={pageSize}+search={search}+currentPage={currentPage} 
        /// </summary>
        Task<Response?> Get(int pageSize, int currentPage, string? search);

        Task<Response?> CreateAsync(Product product);
        Task<Response?> UpdateAsync(Product product);
        Task<Response?> DeleteAsync(string id);
    }
}