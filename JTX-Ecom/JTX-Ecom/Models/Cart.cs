using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ConcertId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketType { get; set; } = "General"; // General, VIP, VVIP

        [Required]
        public decimal PricePerTicket { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Cart? Cart { get; set; }
        public virtual Concert? Concert { get; set; }
    }
}
