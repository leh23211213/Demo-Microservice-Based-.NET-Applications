using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.AuthAPI.Models;
[NotMapped]
public class RegistrationRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}