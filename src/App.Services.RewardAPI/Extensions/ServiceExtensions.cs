namespace App.Services.ProductAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            // var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            // services.AddSingleton(new RewardService(optionBuilder.Options));

            // services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
            return services;
        }
    }
}