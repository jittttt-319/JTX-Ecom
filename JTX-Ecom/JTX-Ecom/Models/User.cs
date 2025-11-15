using Microsoft.AspNetCore.Identity;

namespace JTX_Ecom.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual UserProfile? Profile { get; set; }
        public virtual Cart? Cart { get; set; }
    }
}
