using System.Text;
using App.Services.ShoppingCartAPI.Models;
using App.Services.ShoppingCartAPI.Services;
using App.Services.ShoppingCartAPI.Services.IServices;
using App.Services.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.ShoppingCartAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ApiAuthenticationHttpClientHandler>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Response>();
            services.AddHttpContextAccessor();
            services.AddHttpClient("Product", u => u.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<ApiAuthenticationHttpClientHandler>();

            return services;
        }
    }
}