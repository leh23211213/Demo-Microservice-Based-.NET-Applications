using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Services.IServices;
using Newtonsoft.Json;

namespace App.Services.OrderAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<IEnumerable<Product>> Get()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/v1/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<Response>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Product>>(Convert.ToString(resp.Result));
            }
            return new List<Product>();
        }
    }
}