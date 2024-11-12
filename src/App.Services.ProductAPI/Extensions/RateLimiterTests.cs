using Xunit;
namespace App.Services.ProductAPI.Extensions
{
    /// <summary>
    /// dotnet test
    /// </summary>
    public class RateLimiterTests
    {
        [Fact]
        public async Task RateLimiter_AllowsOnlyLimitedRequests()
        {
            var redisConnectionString = "localhost:6379";
            var rateLimiter = new RedisRateLimiter(redisConnectionString, requestLimit: 10, timeWindow: TimeSpan.FromSeconds(10));

            var userId = "test-user";
            int allowedRequests = 0;
            int totalRequests = 100;

            for (int i = 0; i < totalRequests; i++)
            {
                bool isAllowed = await rateLimiter.IsRequestAllowedAsync(userId);
                if (isAllowed) allowedRequests++;
            }

            // Assert that allowed requests are within the defined limit
            Assert.True(allowedRequests <= 10, $"Allowed requests: {allowedRequests} exceeded the limit");
        }
    }
}