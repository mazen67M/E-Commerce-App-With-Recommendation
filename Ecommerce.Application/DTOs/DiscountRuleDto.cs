using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class DiscountRuleDto
    {
        public int RuleId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public int? PromoCodeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public decimal? MinimumOrderAmount { get; set; }

    }
}
