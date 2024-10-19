namespace App.Frontend.Models
{
    public class StripeRequest
    {
        public string? StripeSessionId { get; set; }
        public string? StripeSessionUrl { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}