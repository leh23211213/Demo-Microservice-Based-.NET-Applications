﻿using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace App.Services.ShoppingCartAPI.Utility
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
            var token = await _accessor.HttpContext.GetTokenAsync("access_token");

            request.Headers.Authorization = new AuthenticationHeaderValue("token", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
