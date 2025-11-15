using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models.DTOs
{
    public class AddToCartDto
    {
        [Required]
        public int ConcertId { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketType { get; set; } = "General";
    }

    public class UpdateCartItemDto
    {
        [Required]
        public int CartItemId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }
    }

    public class CartSummaryDto
    {
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }

    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ConcertId { get; set; }
        public string ConcertTitle { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public string TicketType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal Subtotal { get; set; }
        public string? ImageUrl { get; set; }
    }
}
