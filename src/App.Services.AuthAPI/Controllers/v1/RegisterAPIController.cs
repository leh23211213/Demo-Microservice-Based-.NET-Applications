using System.Net;
using App.Services.Bus;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;

namespace App.Services.AuthAPI.Controllers
{
    [ApiController]
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/auth")]
    public class RegisterAPIController : ControllerBase
    {
        protected Response _response;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private readonly IAuthAPIService _authAPIService;

        public RegisterAPIController(
                                    IMessageBus messageBus,
                                    IConfiguration configuration,
                                    IAuthAPIService authAPIService
                                    )
        {
            _response = new();
            _messageBus = messageBus;
            _configuration = configuration;
            _authAPIService = authAPIService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register([FromBody] RegistrationRequest model)
        {
            var errorMessage = await _authAPIService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return _response;
            }
            await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
            return _response;
        }
    }
}