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

        [HttpPost("Register")]
        public async Task<ActionResult<Response>> Register([FromBody] RegistrationRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var errorMessage = await _authAPIService.Register(model);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        _response.IsSuccess = false;
                        _response.Message = errorMessage;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return _response;
                    }
                    //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
                    _response.Message = "Registration completed successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Input invalid";
                    _response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }
    }
}