using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class Shipping
    {
        public int ShippingID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        public string CourierName { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
