using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Areas.Account.Models;
public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string ProviderDisplayName { get; set; }
    public string ReturnUrl { get; set; }
    [TempData]
    public string ErrorMessage { get; set; }
}
