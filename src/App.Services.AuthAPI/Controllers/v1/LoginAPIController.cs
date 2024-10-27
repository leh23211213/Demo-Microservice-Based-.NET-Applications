using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;

namespace App.Services.AuthAPI.Controllers
{
    [ApiController]
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/auth")]
    public class LoginAPIController : ControllerBase
    {
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public LoginAPIController(
                                IAuthAPIService authAPIService
                                )
        {
            _response = new();
            _authAPIService = authAPIService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Response>> Login([FromBody] LoginRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = await _authAPIService.Login(model);
                    if (token == null || string.IsNullOrEmpty(token.AccessToken))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = "Email or password is incorrect";
                        return _response;
                    }
                    _response.Result = token;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.Unauthorized;
            }
            return _response;
        }

        [HttpPost("Revoke")]
        public async Task<ActionResult<Response>> RevokeRefreshToken([FromBody] Token token)
        {
            if (ModelState.IsValid)
            {
                await _authAPIService.RevokeRefreshToken(token);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Result = "Invalid Input";
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }
    }
}