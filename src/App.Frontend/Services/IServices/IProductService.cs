using App.Frontend.Models;
namespace App.Frontend.Services.IServices
{
    public interface IProductService
    {
        Task<Response> GetAsync(string search, int currentPage);
        Task<Response> GetAsync(string id);
        Task<Response> CreateAsync(Product product, string token);
        Task<Response> UpdateAsync(Product product, string token);
        Task<Response> Delete(string id, string token);
    }
}