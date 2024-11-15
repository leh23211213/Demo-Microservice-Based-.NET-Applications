<<<<<<< HEAD
namespace App.Domain.Admin.Areas.Account.Models
{
    public class AccountErrorModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Message { get; set; } = string.Empty;
    }
=======
namespace App.Frontend.Areas.Account.Models;
public class AccountErrorModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string Message { get; set; } = string.Empty;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
}
