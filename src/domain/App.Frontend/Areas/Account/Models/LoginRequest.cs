using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

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

    [Required(ErrorMessage = "Security code is required.")]
    [StringLength(4, ErrorMessage = "The code must be exactly 4 digits.", MinimumLength = 4)]
    public string? EnteredCode { get; set; }

    // The code that will be generated and compared during validation.
    [HiddenInput]
    public string? GeneratedCode { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    public string? ReturnUrl { get; set; }
}