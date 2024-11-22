namespace App.Frontend.Extensions
{
    public class RequestTimeout
    {
        private readonly RequestDelegate _next;
        private readonly TimeSpan _timeout;

        public RequestTimeout(RequestDelegate next, TimeSpan timeout)
        {
            _next = next;
            _timeout = timeout;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var cts = new CancellationTokenSource(_timeout);
            context.RequestAborted = cts.Token;

            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested)
            {
                context.Response.Redirect("/Error/RequestTimeout");
                //   context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
            }
        }
    }

    public static class RequestTimeoutMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimeout(this IApplicationBuilder builder, TimeSpan timeout)
        {
            return builder.UseMiddleware<RequestTimeout>(timeout);
        }
    }
}