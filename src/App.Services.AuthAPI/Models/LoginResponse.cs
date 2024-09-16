using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.AuthAPI.Models;
[NotMapped]
public class LoginResponse
{
    public User User { get; set; }
    public string Token { get; set; }
}
