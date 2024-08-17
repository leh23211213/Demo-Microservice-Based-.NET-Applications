using Microsoft.AspNetCore.Identity;

namespace App.Data.Models;

public class User : IdentityUser
{
    public DateTime? BirthDate { get; set; }
    public virtual Cart? Cart { get; set; } = null!;
}
