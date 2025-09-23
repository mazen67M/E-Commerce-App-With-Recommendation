using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Shipping
{
    public class ShippingDto
    {
        public int ShippingID { get; set; }
        public int OrderID { get; set; }
        public string? CourierName { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
