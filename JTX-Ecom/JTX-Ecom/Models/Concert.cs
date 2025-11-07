using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models
{
    public class Concert
    {
        [Key]
        public int ConcertId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public DateTime EventTime { get; set; }

        [Required]
        [StringLength(100)]
        public string Artist { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal BasePrice { get; set; }

        [Required]
        public int AvailableTickets { get; set; }

        [Required]
        public int TotalTickets { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public int VenueId { get; set; }
        public virtual Venue? Venue { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
