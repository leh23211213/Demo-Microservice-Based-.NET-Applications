
// using App.Services.AuthAPI.Models;
// using App.Services.AuthAPI.Services.IServices;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;

// namespace App.Services.AuthAPI.Service
// {
//     public class JwtTokenGenerator : IJwtTokenGenerator
//     {
//         private string secretKey;
//         public JwtTokenGenerator(IConfiguration configuration)
//         {
//             secretKey = configuration.GetValue<string>("ApiSettings:Secret");
//         }

//         public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
//         {
//             var tokenHandler = new JwtSecurityTokenHandler();

//             var key = Encoding.ASCII.GetBytes(secretKey);

//             var claimList = new List<Claim>
//             {
//                 new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email),
//                 new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id),
//                 new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName)
//             };

//             claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

//             var tokenDescriptor = new SecurityTokenDescriptor
//             {
//                 //Issuer = "",
//                 //Audience = "",
//                 Subject = new ClaimsIdentity(claimList),
//                 Expires = DateTime.UtcNow.AddDays(7),
//                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//             };

//             var token = tokenHandler.CreateToken(tokenDescriptor);
//             return tokenHandler.WriteToken(token);
//         }
//     }
// }
