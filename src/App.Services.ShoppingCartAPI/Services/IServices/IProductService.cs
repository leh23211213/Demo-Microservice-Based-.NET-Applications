using App.Services.ShoppingCartAPI.Models;

namespace App.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAsync();
    }
}