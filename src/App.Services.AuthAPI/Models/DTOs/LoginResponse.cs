
namespace App.Services.AuthAPI.Models.DTOs;
public class LoginResponse
{
    public UserDTO UserDTO { get; set; }
    public string Token { get; set; }
}