using System.Net;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersionNeutral]
    public class LoginAPIController : ControllerBase
    {
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public LoginAPIController(IAuthAPIService authAPIService)
        {
            _authAPIService = authAPIService;
            _response = new Response();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _authAPIService.Login(model);
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = token;
            return Ok(_response);
        }
    }
}