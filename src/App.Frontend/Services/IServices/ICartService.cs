using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface ICartService
    {
        Task<Response> GetAsync(string userId);
        Task<Response> RemoveAsync(int cartDetailsId);
        Task<Response> AddAsync(Cart cart);
    }
}