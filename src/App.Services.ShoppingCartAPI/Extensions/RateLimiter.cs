using System.Threading.RateLimiting;

namespace App.Services.ShoppingCartAPI;
public static class RateLimiter
{
    public static IServiceCollection AddRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("RateLimitPolicy", context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "default",
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 50, // Giới hạn n requests
                        Window = TimeSpan.FromSeconds(10), // Reset sau n giây
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0 // Không xếp hàng
                    });
            });
        });

        return services;
    }
}