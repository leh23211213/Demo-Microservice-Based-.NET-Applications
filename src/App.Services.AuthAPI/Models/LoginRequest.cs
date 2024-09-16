using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.AuthAPI.Models;
[NotMapped]
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}