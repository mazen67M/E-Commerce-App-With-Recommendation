using Ecommerce.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemDto> Items { get; set; } = new();
        public decimal SubTotal => Items.Sum(i => i.UnitPrice * i.Quantity);
        public decimal Tax { get; set; } // Could be calculated
        public decimal Shipping { get; set; }
        public decimal Total => SubTotal + Tax + Shipping;
        public string? PromoCode { get; set; }
        public decimal Discount { get; set; }
    }
}
