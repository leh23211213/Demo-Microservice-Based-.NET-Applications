
using System.Net;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersionNeutral]
    public class TokenController : Controller
    {
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public TokenController(IAuthAPIService authAPIService)
        {
            _response = new Response();
            _authAPIService = authAPIService;
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] Token token)
        {
            if (ModelState.IsValid)
            {
                var tokenResponse = await _authAPIService.RefreshAccessToken(token);
                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Message = "Token Invalid";
                    return BadRequest(_response);
                }
                _response.Result = tokenResponse;
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Result = "Invalid Input";
                return BadRequest(_response);
            }
        }
    }
}