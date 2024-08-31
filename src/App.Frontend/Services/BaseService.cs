
using System.Net;
using System.Text;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using static App.Frontend.Utility.StaticDetail;

namespace App.Frontend.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Response?> SendAsync(Request request, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("AppAPI");
                HttpRequestMessage message = new();
                
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.Url);

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
                
                HttpResponseMessage? apiResponse = null;

                switch (request.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var Response = JsonConvert.DeserializeObject<Response>(apiContent);
                        return Response;
                }
            }
            catch (Exception ex)
            {
                var newResponse = new Response
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return newResponse;
            }

        }
    }
}