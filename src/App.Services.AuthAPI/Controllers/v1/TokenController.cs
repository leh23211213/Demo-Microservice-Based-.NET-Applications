
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
        protected Response _response;
        private readonly IAuthAPIService _authAPIService;
        public TokenController(IAuthAPIService authAPIService)
        {
            _response = new Response();
            _authAPIService = authAPIService;
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<Response>> GetNewTokenFromRefreshToken([FromBody] Token token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tokenResponse = await _authAPIService.RefreshAccessToken(token);
                    if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = "Token Invalid";
                        return _response;
                    }
                    _response.Result = tokenResponse;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }
    }
}