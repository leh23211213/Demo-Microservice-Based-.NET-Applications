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
        protected Response _response;
        private readonly IAuthAPIService _authAPIService;

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
            if (ModelState.IsValid)
            {
                var token = await _authAPIService.Login(model);
                if (token == null || string.IsNullOrEmpty(token.AccessToken))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email or password is incorrect";
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                _response.Result = token;
            }
            return _response;
        }
    }
}