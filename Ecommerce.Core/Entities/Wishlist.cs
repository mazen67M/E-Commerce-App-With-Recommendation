using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class Wishlist
    {
        public int WishlistID { get; set; }

        [Required]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}
