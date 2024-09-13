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
            services.AddScoped<ApiAuthenticationHttpClientHandle>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Response>();

            services.AddHttpContextAccessor();
            services.AddHttpClient("Product", u => u.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<ApiAuthenticationHttpClientHandle>();

            var settingsSection = configuration.GetSection("ApiSettings");

            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");
            var key = Encoding.UTF8.GetBytes(secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = audience,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            services.AddAuthorization();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthentication();
            return services;
        }
    }
}