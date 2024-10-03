
using App.Services.OrderAPI.Models;

namespace App.Services.OrderAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> Get();
    }
}