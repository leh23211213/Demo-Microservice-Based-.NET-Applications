// using StackExchange.Redis;
// namespace App.Services.ProductAPI.Extensions
// {
//     /*
//     How It Works
//         Redis Key Expiration:

//         When the first request in a 10-second window is made, KeyExpireAsync sets an expiration of 10 seconds on the key (defined by _timeWindow).
//         Redis automatically deletes the key when it expires (10 seconds later), resetting the counter to 0.
//         Reset Trigger:

//         The next time the user makes a request after 10 seconds, the counter restarts from 1 because the previous key was removed. This means the counter resets automatically every 10 seconds.
//         Detailed Example
//         Let’s walk through an example of how it behaves over time:

//         First Request: requestCount is 1, and KeyExpireAsync sets an expiration of 10 seconds.
//         Next 9 Requests within 10 seconds: Increment requestCount up to 10, without resetting expiration.
//         11th Request within 10 seconds: requestCount is 11, which exceeds the limit, so the request is denied.
//         After 10 Seconds: Redis expires the key, so the counter is deleted.
//         New Request after Expiration: Since the key no longer exists, the counter restarts at 1.
//         This approach automatically resets the request count to 0 every 10 seconds without additional code, leveraging Redis key expiration.
//     */
//     public class RedisRateLimiter
//     {
//         private readonly ConnectionMultiplexer _redis;
//         private readonly int _requestLimit = 10;
//         private readonly TimeSpan _timeWindow = TimeSpan.FromSeconds(10);
//         public RedisRateLimiter(string redisConnectionString)
//         {
//             _redis = ConnectionMultiplexer.Connect(redisConnectionString);
//         }

//         public async Task<bool> IsRequestAllowedAsync(string userId)
//         {
//             var db = _redis.GetDatabase();
//             var key = $"rate_limit:{userId}";

//             // Increment the request count for this user
//             var requestCount = await db.StringIncrementAsync(key);

//             // Set the expiration for the key if it's the first request
//             if (requestCount == 1)
//             {
//                 await db.KeyExpireAsync(key, _timeWindow);
//             }

//             return requestCount <= _requestLimit; // Allow the request
//         }
//     }
// }