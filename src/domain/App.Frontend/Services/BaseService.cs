
using System.Net;
using System.Text;
using Newtonsoft.Json;
using App.Frontend.Models;
using App.Frontend.Utility;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace App.Frontend.Services
{
    public class BaseService : IBaseService
    {
        protected readonly string _url = null!;
        private readonly ITokenProvider _tokenProvider;
        private IHttpContextAccessor _httpContextAccessor;
        public IHttpClientFactory _httpClientFactory { get; set; }
        private readonly IApiMessageRequestBuilder _apiMessageRequestBuilder;
        public BaseService(
                            IConfiguration configuration,
                            ITokenProvider tokenProvider,
                            IHttpClientFactory httpClientFactory,
                            IHttpContextAccessor httpContextAccessor,
                            IApiMessageRequestBuilder apiMessageRequestBuilder)
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _apiMessageRequestBuilder = apiMessageRequestBuilder;
            _url = configuration.GetValue<string>("ServiceUrls:authAPI");
        }

        public async Task<Response?> SendAsync(Request request, bool withBearer = true)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApplicationAPI");
                var messageFactory = () =>
                {
                    return _apiMessageRequestBuilder.Build(request);
                };

                HttpResponseMessage httpResponseMessage = null;
                httpResponseMessage = await SendWithRefreshTokenAsync(client, messageFactory, withBearer);
                Response FinalApiResponse = new(){ IsSuccess = false };

                try
                {
                    switch (httpResponseMessage.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            FinalApiResponse.Message = "Not Found";
                            break;
                        case HttpStatusCode.Forbidden:
                            FinalApiResponse.Message = "Access Denied";
                            break;
                        case HttpStatusCode.Unauthorized:
                            FinalApiResponse.Message = "Unauthorized";
                            break;
                        case HttpStatusCode.InternalServerError:
                            FinalApiResponse.Message = "Internal Server Error";
                            break;
                        default:
                            var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                            FinalApiResponse.IsSuccess = true;
                            FinalApiResponse = JsonConvert.DeserializeObject<Response>(apiContent);
                            break;
                    }
                }
                catch (Exception e)
                {
                    FinalApiResponse.Message = e.Message.ToString();
                }
                var res = JsonConvert.SerializeObject(FinalApiResponse);
                var returnObj = JsonConvert.DeserializeObject<Response>(res);
                return returnObj;
            }
            catch (AuthException)
            {
                throw;
            }
            catch (Exception e)
            {
                var dto = new Response
                {
                    Message = Convert.ToString(e.Message),
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var Response = JsonConvert.DeserializeObject<Response>(res);
                return Response;
            }
        }

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient,
            Func<HttpRequestMessage> httpRequestMessageFactory, bool withBearer = true)
        {
            if (!withBearer)
            {
                return await httpClient.SendAsync(httpRequestMessageFactory());
            }
            else
            {
                Token token = _tokenProvider.GetToken();
                if (token != null && !string.IsNullOrEmpty(token.AccessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }
                try
                {
                    var response = await httpClient.SendAsync(httpRequestMessageFactory());
                    if (response.IsSuccessStatusCode)
                        return response;
                    // IF this fails then we can pass refresh token!
                    if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //GENERATE NEW Token from Refresh token / Sign in with that new token and then retry
                        await InvokeRefreshTokenEndpoint(httpClient, token.AccessToken, token.RefreshToken);
                        response = await httpClient.SendAsync(httpRequestMessageFactory());
                        return response;
                    }
                    return response;
                }
                catch (AuthException)
                {
                    throw;
                }
                catch (HttpRequestException httpRequestException)
                {
                    if (httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // refresh token and retry the request
                        await InvokeRefreshTokenEndpoint(httpClient, token.AccessToken, token.RefreshToken);
                        return await httpClient.SendAsync(httpRequestMessageFactory());
                    }
                    throw;
                }
            }
        }

        private async Task InvokeRefreshTokenEndpoint(HttpClient httpClient, string existingAccessToken, string existingRefreshToken)
        {
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri($"{_url}/api/{StaticDetail.CurrentAPIVersion}/auth/Refresh");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(JsonConvert.SerializeObject(new Token()
            {
                AccessToken = existingAccessToken,
                RefreshToken = existingRefreshToken
            }), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<Response>(content);

            if (apiResponse?.IsSuccess != true)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
                throw new AuthException();
            }
            else
            {
                var tokenDataStr = JsonConvert.SerializeObject(apiResponse.Result);
                var token = JsonConvert.DeserializeObject<Token>(tokenDataStr);

                if (token != null && !string.IsNullOrEmpty(token.AccessToken))
                {
                    //New method to sign in with the new token that we receive
                    await SignInWithNewTokens(token);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }
            }
        }

        private async Task SignInWithNewTokens(Token token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token.AccessToken);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            // for render information
            identity.AddClaim(new Claim(ClaimTypes.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = true, // Cookie sẽ tồn tại qua nhiều phiên duyệt web
            });

            _tokenProvider.SetToken(token);
        }
    }
}
