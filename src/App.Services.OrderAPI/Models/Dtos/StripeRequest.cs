namespace App.Services.OrderAPI.Models
{
    public class StripeRequest
    {
        public string? Id { get; set; }
        public string? StripeSessionUrl { get; set; }
        public string? ApprovedUrl { get; set; }
        public string? CancelUrl { get; set; }
        public OrderHeader? OrderHeader { get; set; }
    }
}