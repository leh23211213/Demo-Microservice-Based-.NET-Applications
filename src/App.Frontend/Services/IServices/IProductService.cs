using App.Frontend.Models;
namespace App.Frontend.Services.IServices
{
    public interface IProductService
    {
        Task<Response> GetAsync(int currentPage, string token);
        Task<Response> GetAsync(string id, string token);
        Task<Response> CreateAsync(Product product, string token);
        Task<Response> UpdateAsync(Product product, string token);
        Task<Response> Delete(string id, string token);
    }
}