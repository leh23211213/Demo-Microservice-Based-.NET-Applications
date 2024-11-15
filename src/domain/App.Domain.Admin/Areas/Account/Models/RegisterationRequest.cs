using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
namespace App.Domain.Admin.Areas.Account.Models;
public class RegistrationRequest
{
    [Required]
    [EmailAddress]
<<<<<<< HEAD
    [Display(Name = "Email")]
=======
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
<<<<<<< HEAD
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
=======
    public string Password { get; set; }

    [DataType(DataType.Password)]
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string PhoneNumber { get; set; }

    public string Role { get; set; }
    public IEnumerable<AuthenticationScheme>? ExternalLogins { get; set; }
}