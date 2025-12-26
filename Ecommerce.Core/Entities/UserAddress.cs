using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Entities
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, MaxLength(100)]
        public string AddressName { get; set; } = "Home"; // e.g., "Home", "Work", "Mom's House"

        [Required, MaxLength(100)]
        public string RecipientName { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(200)]
        public string AddressLine1 { get; set; }

        [MaxLength(200)]
        public string? AddressLine2 { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [Required, MaxLength(20)]
        public string PostalCode { get; set; }

        [Required, MaxLength(100)]
        public string Country { get; set; } = "Egypt";

        public bool IsDefaultShipping { get; set; } = false;
        public bool IsDefaultBilling { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
