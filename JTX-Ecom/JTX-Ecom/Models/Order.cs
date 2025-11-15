using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000.00)]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded

        [StringLength(50)]
        public string? PaymentMethod { get; set; } // FPX, Credit Card, Debit Card, TNG eWallet, GrabPay, Boost

        [StringLength(200)]
        public string? TransactionId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? PaymentDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Malaysian-specific fields
        [StringLength(200)]
        public string? BillingAddress { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        // User relationship
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        // Navigation properties
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
