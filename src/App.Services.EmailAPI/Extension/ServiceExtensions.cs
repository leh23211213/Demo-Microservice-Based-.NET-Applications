using App.Services.EmailAPI.Messaging;
using App.Services.EmailAPI.Services;
using App.Services.EmailAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace App.Services.EmailAPI.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton(new EmailService(optionBuilder.Options));

            services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

            return services;
        }
    }
}