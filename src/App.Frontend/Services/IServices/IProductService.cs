using App.Frontend.Models;
namespace App.Frontend.Services.IServices
{
    public interface IProductService
    {
        Task<Response> GetAsync(int currentPage);
        Task<Response> GetAsync(string id);
        Task<Response> CreateAsync(Product product);
        Task<Response> UpdateAsync(Product product);
        Task<Response> Delete(string id);
    }
}