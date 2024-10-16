using App.Services.EmailAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace App.Services.EmailAPI.Extension
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