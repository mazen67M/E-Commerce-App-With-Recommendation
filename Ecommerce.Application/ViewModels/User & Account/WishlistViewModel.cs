using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.DTOs.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.User___Account
{
    public class WishlistViewModel
    {
        public List<WishlistItemDto> Items { get; set; } = new();
    }
}
