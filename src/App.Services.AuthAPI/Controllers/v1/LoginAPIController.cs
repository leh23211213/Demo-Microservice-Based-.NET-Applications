using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.AuthAPI.Controllers
{
    [ApiController]
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/auth")]
    public class LoginAPIController : ControllerBase
    {
        protected Response _response;
        private readonly string issuer;
        private readonly string audience;
        private readonly string secretKey;
        private readonly ILoginAPIService _loginAPIService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;
        public LoginAPIController(
                                IConfiguration configuration,
                                ILoginAPIService loginAPIService,
                                ITokenProvider tokenProvider
                                )
        {
            _response = new();
            _loginAPIService = loginAPIService;
            _tokenProvider = tokenProvider;

            _configuration = configuration;

            issuer = _configuration.GetValue<string>("SetToken:Issuer");
            audience = _configuration.GetValue<string>("SetToken:Audience");
            secretKey = _configuration.GetValue<string>("SetToken:Secret");
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Response>> Login([FromBody] LoginRequest model)
        {
            if (model is null) return _response;

            if (ModelState.IsValid)
            {
                try
                {
                    // create token
                    var token = await _loginAPIService.Login(model);
                    if (token == null || string.IsNullOrEmpty(token.AccessToken))
                    {
                        _response.Message = "Email or password is incorrect";
                        return _response;
                    }
                    // Validate token
                    if (Validate(token.AccessToken))
                    {
                        _response.Result = token;
                        _tokenProvider.SetToken(token);
                    }
                }
                catch (Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Internal Server Error";
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                }
            }
            return _response;
        }

        private bool Validate(string ac_tk)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            try
            {
                // Validate token
                var principal = tokenHandler.ValidateToken(ac_tk, new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            // catch (SecurityTokenExpiredException)
            // {
            //     new SecurityTokenExpiredException("Token has expired.");
            // }
            // catch (SecurityTokenNotYetValidException)
            // {
            //     new SecurityTokenNotYetValidException("Token is not yet valid.");
            // }
            // catch (SecurityTokenInvalidSignatureException)
            // {
            //     new SecurityTokenInvalidSignatureException("Invalid signature.");
            // }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }
            return false;
        }

    }
}