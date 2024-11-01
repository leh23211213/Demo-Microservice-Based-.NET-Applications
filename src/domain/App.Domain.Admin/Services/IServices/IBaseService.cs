
using App.Domain.Admin.Models;

namespace App.Domain.Admin.Services.IServices
{
    public interface IBaseService
    {
        Task<Response?> SendAsync(Request Request, bool withBearer = true);
    }
}
