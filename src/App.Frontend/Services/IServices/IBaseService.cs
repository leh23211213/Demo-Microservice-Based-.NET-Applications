
using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IBaseService
    {
        Response _response { get; set; }
        Task<Response?> SendAsync(Request Request, bool withBearer = true);
    }
}
