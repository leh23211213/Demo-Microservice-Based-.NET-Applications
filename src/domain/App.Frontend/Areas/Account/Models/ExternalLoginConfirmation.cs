using System.ComponentModel.DataAnnotations;

namespace App.Frontend.Areas.Account.Models;
public class ExternalLoginConfirmation
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
