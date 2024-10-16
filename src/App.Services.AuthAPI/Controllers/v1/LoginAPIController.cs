using System.Net;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using App.Services.Bus;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersionNeutral]
    public class LoginAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public LoginAPIController(
                                IAuthAPIService authAPIService,
                                   IMessageBus messageBus,
                                    IConfiguration configuration
                                )
        {
            _authAPIService = authAPIService;
            _response = new();
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _authAPIService.Login(model);
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = "Email or password is incorrect";
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = token;
            return Ok(_response);
        }
    }
}