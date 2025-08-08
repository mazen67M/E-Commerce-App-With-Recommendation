using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Entities
{
    public class OrderPromoCode
    {
        public int OrderPromoCodeID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public int PromoCodeID { get; set; }
        public virtual PromoCode PromoCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountApplied { get; set; }
    }
}
