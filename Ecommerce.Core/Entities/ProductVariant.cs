using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Entities
{
    /// <summary>
    /// Product variants (Size, Color, etc.)
    /// </summary>
    public class ProductVariant
    {
        [Key]
        public int VariantID { get; set; }

        public int ProductID { get; set; }

        [Required, MaxLength(50)]
        public string VariantType { get; set; } = string.Empty; // "Size", "Color", etc.

        [Required, MaxLength(100)]
        public string VariantValue { get; set; } = string.Empty; // "Large", "Red", etc.

        [MaxLength(50)]
        public string? SKU { get; set; }

        // Price adjustment (can be positive or negative)
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAdjustment { get; set; } = 0;

        public int StockQuantity { get; set; } = 0;

        public bool IsAvailable { get; set; } = true;

        // For color variants, store the hex color code
        [MaxLength(7)]
        public string? ColorCode { get; set; }

        // For size variants, store display order
        public int DisplayOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; } = null!;
    }
}
