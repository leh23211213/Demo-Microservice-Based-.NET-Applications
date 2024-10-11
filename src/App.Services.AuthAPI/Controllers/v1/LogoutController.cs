using System.Net;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersionNeutral]
    public class LogoutController : ControllerBase
    {
        private readonly IAuthAPIService _authAPIService;
        protected Response _response;

        public LogoutController(IAuthAPIService authAPIService)
        {
            _response = new Response();
            _authAPIService = authAPIService;
        }


        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] Token token)
        {

            if (ModelState.IsValid)
            {
                await _authAPIService.RevokeRefreshToken(token);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            _response.IsSuccess = false;
            _response.Result = "Invalid Input";
            return BadRequest(_response);
        }
    }
}