using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.AuthAPI.Models;
[NotMapped]
public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}