using App.Services.ShoppingCartAPI.Models;

namespace App.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        /// <summary>
        /// Get all product from product api
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAsync();
    }
}