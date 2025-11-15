using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; } = "Malaysia";

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string? ICNumber { get; set; } // Malaysian IC Number

        [StringLength(500)]
        public string? ProfilePicture { get; set; }

        public bool ReceiveNewsletter { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public virtual User? User { get; set; }
    }
}
