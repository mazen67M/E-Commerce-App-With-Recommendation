using Ecommerce.Core.Enums;

namespace Ecommerce.Application.DTOs.Promotion
{
    public class DiscountRuleDto
    {
        public int RuleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType Type { get; set; }

        public decimal Value { get; set; }
        public int? PromoCodeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
    }
}