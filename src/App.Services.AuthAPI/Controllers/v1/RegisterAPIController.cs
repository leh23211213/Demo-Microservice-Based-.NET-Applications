using System.Net;
using App.Services.Bus;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services;
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
        private readonly IRegisterAPIService _registerAPIService;

        public RegisterAPIController(
                                    IMessageBus messageBus,
                                    IConfiguration configuration,
                                    IRegisterAPIService registerAPIService
                                    )
        {
            _response = new();
            _messageBus = messageBus;
            _configuration = configuration;
            _registerAPIService = registerAPIService;
        }


        [HttpPost("Register")]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<Response>> Register([FromBody] RegistrationRequest model)
        {
            if (model is null) return _response;

            if (ModelState.IsValid)
            {
                var errorMessage = await _registerAPIService.Register(model);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _response.IsSuccess = false;
                    _response.Message = errorMessage;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    return _response;
                }
                //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

                _response.Message = "Registration Successful";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Internal Server Error";
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }
    }
}