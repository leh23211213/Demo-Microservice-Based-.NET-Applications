
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services;
namespace App.Services.AuthAPI.Controllers
{
    [ApiController]
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/auth")]
    public class TokenController : Controller
    {
        protected Response _response;
        private readonly ITokenProvider _tokenProvider;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public TokenController(

                                ITokenProvider tokenProvider,
                                IJwtTokenGenerator jwtTokenGenerator
                            )
        {
            _response = new();
            _tokenProvider = tokenProvider;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<Response>> GetNewTokenFromRefreshToken([FromBody] Token token)
        {
            if (token is null) return _response;

            try
            {
                if (ModelState.IsValid)
                {
                    var tokenResponse = await _jwtTokenGenerator.RefreshAccessToken(token);
                    if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Internal Server Error";
                        _response.StatusCode = HttpStatusCode.InternalServerError;
                        return _response;
                    }
                    _response.Result = tokenResponse;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Internal Server Error";
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }


        [HttpPost("Revoke")]
        public async Task<ActionResult<Response>> RevokeRefreshToken([FromBody] Token token)
        {
            if (token is null) return _response;

            try
            {
                if (ModelState.IsValid)
                {
                    _tokenProvider.ClearToken();
                    await _jwtTokenGenerator.RevokeRefreshToken(token);
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Internal Server Error";
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpDelete("warning-remove-token")]
        public async Task<ActionResult<Response>> warning()
        {
            _tokenProvider.ClearToken();
            _response.Message = " Already Clear Token!!!";
            return _response;
        }

        [HttpGet("warning-gettoken")]
        public async Task<ActionResult<Response>> gettoken()
        {
            _tokenProvider.ClearToken();
            _response.Result = _tokenProvider.GetToken();
            return _response;
        }
    }
}