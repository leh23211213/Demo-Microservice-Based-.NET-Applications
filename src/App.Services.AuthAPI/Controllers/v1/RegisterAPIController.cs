using System.Net;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using App.Services.Bus;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersionNeutral]
    public class RegisterAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public RegisterAPIController(
                                    IAuthAPIService authAPIService,
                                    IMessageBus messageBus,
                                    IConfiguration configuration
                                    )
        {
            _authAPIService = authAPIService;
            _response = new();
            _configuration = configuration;
            _messageBus = messageBus;
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

            await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
            return Ok(_response);
        }
    }
}