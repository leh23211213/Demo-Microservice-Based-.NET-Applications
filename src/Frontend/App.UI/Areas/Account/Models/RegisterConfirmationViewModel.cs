namespace App.UI.Areas.Account.Models;

    public class RegisterConfirmationViewModel
    {
        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }
    }
