namespace App.Services.ProductAPI.Extensions
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<string, DateTime> _clientRequests = new();
        private readonly int _requestLimit = 5;
        private readonly TimeSpan _timeWindow = TimeSpan.FromSeconds(10);

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = context.Connection.RemoteIpAddress?.ToString();
            if (clientId == null || !IsRequestAllowed(clientId))
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }
            await _next(context);
        }

        private bool IsRequestAllowed(string clientId)
        {
            if (!_clientRequests.ContainsKey(clientId))
            {
                _clientRequests[clientId] = DateTime.UtcNow.Add(_timeWindow);
                return true;
            }

            if (_clientRequests[clientId] < DateTime.UtcNow)
            {
                _clientRequests[clientId] = DateTime.UtcNow.Add(_timeWindow);
                return true;
            }
            return false;
        }
    }
}