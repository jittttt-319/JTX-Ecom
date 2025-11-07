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
        [StringLength(50)]
        public string TicketType { get; set; } = "General Admission"; // VIP, General Admission, Premium

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Available"; // Available, Sold, Reserved, Cancelled

        [StringLength(50)]
        public string? SeatNumber { get; set; }

        [StringLength(50)]
        public string? Section { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? SoldAt { get; set; }

        // Navigation properties
        public int ConcertId { get; set; }
        public virtual Concert? Concert { get; set; }

        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
