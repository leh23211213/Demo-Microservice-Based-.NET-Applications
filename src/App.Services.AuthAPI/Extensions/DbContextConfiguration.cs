using App.Services.AuthAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace App.Services.AuthAPI.Extensions
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}