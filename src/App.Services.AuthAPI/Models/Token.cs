using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services.AuthAPI.Models;
[NotMapped]
public class Token
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}