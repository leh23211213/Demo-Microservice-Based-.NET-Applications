using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using App.Services.AuthAPI.Services;
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
        private readonly IConfiguration _configuration;
        private readonly ITokenProvider _tokenProvider;
        private readonly IAuthAPIService _authAPIService;

        public LoginAPIController(
                                IConfiguration configuration,
                                IAuthAPIService authAPIService,
                                ITokenProvider tokenProvider
                                )
        {
            _response = new();
            _authAPIService = authAPIService;
            _tokenProvider = tokenProvider;

            _configuration = configuration;

            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
            issuer = _configuration.GetValue<string>("ApiSettings:Issuer");
            audience = _configuration.GetValue<string>("ApiSettings:Audience");
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Response>> Login([FromBody] LoginRequest model)
        {
            try
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

                    if (Validate(token.AccessToken))
                    {
                        _response.Result = token;
                        _tokenProvider.SetToken(token);
                    }
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
            catch (SecurityTokenExpiredException)
            {
                new SecurityTokenExpiredException("Token has expired.");
            }
            catch (SecurityTokenNotYetValidException)
            {
                new SecurityTokenNotYetValidException("Token is not yet valid.");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                new SecurityTokenInvalidSignatureException("Invalid signature.");
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }
            return false;
        }
    }
}