using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Shipping
{
    public class TrackingInfoDto
    {
        public string TrackingNumber { get; set; } = string.Empty;
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public string CurrentStatus { get; set; } = string.Empty;
        public string StatusDescription { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public List<TrackingEventDto> TrackingHistory { get; set; } = new();
        public string? CarrierTrackingUrl { get; set; }
    }
    public class TrackingEventDto
    {
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

}
