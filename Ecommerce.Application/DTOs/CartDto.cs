using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class CartDto
    {
        public int CartID { get; set; }
        public string UserID { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal SubTotal => Items.Sum(i => i.Total);
        public decimal Tax { get; set; } = 0;
        public decimal Shipping { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal Total => SubTotal + Tax + Shipping - Discount;
    }
}
