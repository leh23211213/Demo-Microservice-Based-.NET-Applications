using Microsoft.AspNetCore.Mvc;

namespace App.UI.Areas.Account.Models;

public class ConfirmEmailModel
{
    [TempData]
    public string StatusMessage { get; set; }
}
