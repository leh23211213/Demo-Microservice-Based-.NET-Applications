
using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IBaseService
    {
        Task<Response?> SendAsync(Request request, bool withBearer = true);
    }
}