
using Microsoft.EntityFrameworkCore;

namespace App.Services.RewardAPI.Extensions
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