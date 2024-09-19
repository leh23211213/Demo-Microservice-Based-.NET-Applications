using System.Net;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersionNeutral]
    public class RegisterAPIController : ControllerBase
    {
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public RegisterAPIController(IAuthAPIService authAPIService)
        {
            _authAPIService = authAPIService;
            _response = new Response();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            var errorMessage = await _authAPIService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}