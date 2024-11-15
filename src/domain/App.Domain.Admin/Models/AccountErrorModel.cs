namespace App.Domain.Admin.Models;
public class AccountErrorModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string Message { get; set; } = string.Empty;
}
