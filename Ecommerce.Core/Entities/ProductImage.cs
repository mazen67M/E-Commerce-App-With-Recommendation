using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Entities
{
    public class ProductImage
    {
        [Key]
        public int ImageID { get; set; }

        public int ProductID { get; set; }

        [Required]
        public string ImageURL { get; set; } = string.Empty;

        public string? AltText { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsPrimary { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; } = null!;
    }
}
