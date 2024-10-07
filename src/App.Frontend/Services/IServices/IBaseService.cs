
using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IBaseService
    {
        Task<Response?> SendAsync(Request Request, bool withBearer = true);
    }
}
