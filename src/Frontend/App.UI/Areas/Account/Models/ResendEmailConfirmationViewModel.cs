
using System.ComponentModel.DataAnnotations;

namespace App.UI.Areas.Account.Models;

    public class ResendEmailConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
