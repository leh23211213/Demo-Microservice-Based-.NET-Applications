namespace App.Domain.Admin.Models
{
    public class Token
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
