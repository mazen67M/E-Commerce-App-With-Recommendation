﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Cart
{
    public class CartItemDto
    {
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageURL { get; set; } 
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}
