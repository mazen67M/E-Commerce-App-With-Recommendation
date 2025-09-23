using Ecommerce.Core.Enums;
using System;

namespace Ecommerce.Application.DTOs.Promotion
{
    public class PromoCodeDto
    {
        public int PromoCodeID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }

        public decimal DiscountValue { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}