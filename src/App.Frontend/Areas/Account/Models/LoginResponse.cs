namespace App.Frontend.Areas.Account.Models;
public class LoginResponse
{
    public User User { get; set; }
    public string Token { get; set; }
}