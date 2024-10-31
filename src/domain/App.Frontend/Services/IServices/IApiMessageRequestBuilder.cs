using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IApiMessageRequestBuilder
    {
        HttpRequestMessage Build(Request Request);
    }
}
