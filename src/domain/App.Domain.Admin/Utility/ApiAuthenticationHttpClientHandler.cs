using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace App.Domain.Admin.Utility
{
    public class ApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
