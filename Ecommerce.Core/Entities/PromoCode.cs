using Ecommerce.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Entities
{
    public class PromoCode
    {
        public int PromoCodeID { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        public string Description { get; set; }

        public DiscountType DiscountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        public int MaxUsage { get; set; } = 1;
        public int UsedCount { get; set; } = 0;

        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<OrderPromoCode> OrderPromoCodes { get; set; } = new List<OrderPromoCode>();
    }
}
