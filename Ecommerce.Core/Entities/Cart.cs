using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class Cart
    {
        public int CartID { get; set; }

        [Required]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
