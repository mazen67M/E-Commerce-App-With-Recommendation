using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Wishlist
{
    public class WishlistDto
    {
        public int WishlistID { get; set; }
        public string UserID { get; set; }
        public List<WishlistItemDto> Items { get; set; } = new();
    }
}
