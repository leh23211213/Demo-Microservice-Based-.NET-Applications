using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace App.Services.ShoppingCartAPI.Utility
{
    public class ApiAuthenticationHttpClientHandle : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public ApiAuthenticationHttpClientHandle(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _accessor.HttpContext.GetTokenAsync("access_token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}