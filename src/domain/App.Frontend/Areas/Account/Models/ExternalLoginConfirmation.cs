using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace App.Frontend.Areas.Account.Models
public class ExternalLoginConfirmation
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
