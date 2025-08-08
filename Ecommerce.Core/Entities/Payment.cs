using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.Enums;

namespace Ecommerce.Core.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string TransactionID { get; set; }
    }
}
