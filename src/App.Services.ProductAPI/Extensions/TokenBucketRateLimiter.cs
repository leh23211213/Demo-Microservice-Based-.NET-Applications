namespace App.Services.ProductAPI.Extensions
{
    public class TokenBucketRateLimiter
    {
        private readonly int _bucketCapacity;
        private readonly int _tokensPerInterval;
        private readonly TimeSpan _interval;
        private int _currentTokens;
        private readonly Timer _timer;

        public TokenBucketRateLimiter(int bucketCapacity, int tokensPerInterval, TimeSpan interval)
        {
            _bucketCapacity = bucketCapacity;
            _tokensPerInterval = tokensPerInterval;
            _interval = interval;
            _currentTokens = bucketCapacity;  // Start with a full bucket

            // Refill tokens periodically
            _timer = new Timer(AddTokens, null, _interval, _interval);
        }

        private void AddTokens(object state)
        {
            // Add tokens at the specified rate, but not exceeding bucket capacity
            _currentTokens = Math.Min(_bucketCapacity, _currentTokens + _tokensPerInterval);
        }

        public bool TryConsumeToken()
        {
            if (_currentTokens > 0)
            {
                Interlocked.Decrement(ref _currentTokens);
                return true;  // Allow the request
            }

            return false;  // Deny the request, no tokens available
        }
    }
}