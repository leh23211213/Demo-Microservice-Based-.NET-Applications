using Microsoft.EntityFrameworkCore;
namespace App.Services.ProductAPI.Extensions
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection")));

            return services;
        }
    }
}