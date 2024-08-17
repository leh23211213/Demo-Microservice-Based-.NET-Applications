using Microsoft.AspNetCore.Identity;

namespace App.UI.Models
{
    public class UserViewModel : IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
