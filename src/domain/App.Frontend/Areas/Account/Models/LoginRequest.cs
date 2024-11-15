

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace App.Frontend.Areas.Account.Models;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
}