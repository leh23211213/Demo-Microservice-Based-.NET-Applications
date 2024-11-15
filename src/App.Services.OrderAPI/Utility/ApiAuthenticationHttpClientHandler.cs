using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace App.Services.OrderAPI.Utility
{
    public class ApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public ApiAuthenticationHttpClientHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
              /*
            {
                "access_token": "...",
                "refresh_token": "",
                "id_token": "...",
                "expires_in": ,
                "token_type": "Bearer",
                "scope": "openid profile email"
            }
            */
            var token = await _accessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
