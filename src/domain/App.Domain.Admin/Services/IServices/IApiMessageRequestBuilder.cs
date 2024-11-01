using App.Domain.Admin.Models;

namespace App.Domain.Admin.Services.IServices
{
    public interface IApiMessageRequestBuilder
    {
        HttpRequestMessage Build(Request Request);
    }
}
