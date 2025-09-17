using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class ShippingRequestDto
    {
        public string FromAddress { get; set; } = string.Empty;
        public string ToAddress { get; set; } = string.Empty;
        public decimal WeightInKg { get; set; }
        public PackageDimensionsDto Dimensions { get; set; } = new();
        public decimal DeclaredValue { get; set; }
        public int? ProviderId { get; set; }
        public string? ServiceLevel { get; set; }
    }

    public class PackageDimensionsDto
    {
        public decimal LengthCm { get; set; }
        public decimal WidthCm { get; set; }
        public decimal HeightCm { get; set; }
        public decimal Volume => LengthCm * WidthCm * HeightCm;
    }
}
