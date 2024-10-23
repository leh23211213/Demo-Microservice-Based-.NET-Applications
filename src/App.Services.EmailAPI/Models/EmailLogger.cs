namespace App.Services.EmailAPI.Models
{
    public class EmailLogger
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime? EmailSentTime { get; set; }

        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
