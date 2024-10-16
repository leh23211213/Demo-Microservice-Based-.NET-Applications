namespace App.Services.OrderAPI.Models.Dtos
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