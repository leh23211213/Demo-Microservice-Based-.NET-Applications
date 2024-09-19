using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;
using Newtonsoft.Json;
using System.Text;
using static App.Frontend.Utility.StaticDetail;

namespace App.Frontend.Services
{
    public class ApiMessageRequestBuilder : IApiMessageRequestBuilder
    {
        public HttpRequestMessage Build(Request Request)
        {
            HttpRequestMessage message = new();
            if (Request.ContentType == ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept", "*/*");
            }
            else
            {
                message.Headers.Add("Accept", "application/json");
            }

            message.RequestUri = new Uri(Request.Url);
            if (Request.ContentType == ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();
                foreach (var prop in Request.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(Request.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                    }
                }
                message.Content = content;
            }
            else
            {
                if (Request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(Request.Data),
                        Encoding.UTF8, "application/json");
                }
            }

            switch (Request.ApiType)
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
            return message;
        }
    }
}
