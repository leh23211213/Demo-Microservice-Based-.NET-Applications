using App.Frontend.Models;
namespace App.Frontend.Services.IServices
{
    public interface IProductService
    {
        Task<Response> GetAsync();
        Task<Response> GetAsync(string id);
        Task<Response> CreateAsync(ProductDTO product);
        Task<Response> UpdateAsync(ProductDTO product);
        Task<Response> Delete(string id);
    }
}