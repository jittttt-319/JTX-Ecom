using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models.DTOs
{
    public class CheckoutDto
    {
        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string BillingAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Postal code must be 5 digits")]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = "FPX"; // FPX, Credit Card, TNG eWallet, GrabPay, Boost
    }

    public class OrderConfirmationDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public string ConcertTitle { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public string TicketType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> TicketNumbers { get; set; } = new();
    }
}
