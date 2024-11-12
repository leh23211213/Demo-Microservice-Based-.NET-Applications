using App.Services.ShoppingCartAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace App.Services.ShoppingCartAPI.Extensions
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection")));

            return services;
        }
    }
}