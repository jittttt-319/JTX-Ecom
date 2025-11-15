using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketNumber { get; set; } = string.Empty;

        [Required]
        public int ConcertId { get; set; }

        public int? OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketType { get; set; } = "General"; // General, VIP, VVIP

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Available"; // Available, Sold, Used, Cancelled

        [StringLength(500)]
        public string? QRCode { get; set; } // QR code for ticket validation

        public DateTime? PurchaseDate { get; set; }

        public DateTime? UsedDate { get; set; }

        [StringLength(200)]
        public string? SeatNumber { get; set; } // Optional seat assignment

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Concert? Concert { get; set; }
        public virtual Order? Order { get; set; }
    }
}
