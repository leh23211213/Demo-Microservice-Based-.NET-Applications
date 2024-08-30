
namespace App.Services.AuthAPI.Models.DTOs;
public class LoginResponse
{
    public User User { get; set; }
    public string Token { get; set; }
}