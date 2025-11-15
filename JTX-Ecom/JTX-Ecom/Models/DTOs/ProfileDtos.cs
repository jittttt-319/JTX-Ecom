using System.ComponentModel.DataAnnotations;

namespace JTX_Ecom.Models.DTOs
{
    public class UserProfileDto
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "Postal code must be 5 digits")]
        public string? PostalCode { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [RegularExpression(@"^\d{6}-\d{2}-\d{4}$", ErrorMessage = "IC Number format: 123456-01-2345")]
        public string? ICNumber { get; set; }

        public bool ReceiveNewsletter { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
