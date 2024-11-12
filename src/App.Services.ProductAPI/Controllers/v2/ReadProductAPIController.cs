
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using App.Services.ProductAPI.Extensions;
using System.Collections.Concurrent;
namespace App.Services.ProductAPI.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/product")]
    public class ReadProductAPIController : Controller
    {
        private Response _response;
        private readonly RedisRateLimiter _rateLimiter;
        private readonly ApplicationDbContext _dbContext;

        public ReadProductAPIController(
                                        IMemoryCache cache,
                                        ApplicationDbContext dbContext,
                                        RedisRateLimiter rateLimiter
                                        )
        {
            _response = new();
            _dbContext = dbContext;
            _rateLimiter = rateLimiter;
        }
        /// <summary>
        /// Extending Token Bucket for Multiple Users
        /// To handle multiple users, you can extend this approach by creating a ConcurrentDictionary of token buckets, where each user has a separate TokenBucketRateLimiter instance:
        /// In this example, each user is identified by userId, and a separate token bucket is managed per user. This way, each user has their own rate limit.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private readonly ConcurrentDictionary<string, TokenBucketRateLimiter> _userBuckets = new();
        public bool TryConsumeTokenForUser(string userId)
        {
            var userLimiter = _userBuckets.GetOrAdd(userId, _ => new TokenBucketRateLimiter(10, 10, TimeSpan.FromSeconds(1)));
            return userLimiter.TryConsumeToken();
        }

        /// <summary>
        /// var rateLimiter = new RedisRateLimiter("localhost:6379", requestLimit: 10, timeWindow: TimeSpan.FromSeconds(1));
        /// builder.Services.AddSingleton(new RedisRateLimiter("localhost:6379", 10, TimeSpan.FromSeconds(1)));
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Get(string userId)
        {
            if (!await _rateLimiter.IsRequestAllowedAsync(userId))
            {
                _response.IsSuccess = false;
                _response.Message = "Too Many Requests";
                _response.StatusCode = HttpStatusCode.Conflict;
            }
            try 
            {
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Result = null;
                //  _logger.LogError(ex.Message);
            }
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> Get(string userId, string id)
        {
            try
            {
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet("Pagination")]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Pagination(
                                                           [FromQuery] int pageSize,
                                                           [FromQuery] int currentPage,
                                                           [FromQuery] string? search = ""
                                                           )
        {
            try
            {
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
