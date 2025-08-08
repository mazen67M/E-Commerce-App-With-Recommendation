using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class CartItem
    {
        public int CartItemID { get; set; }

        [Required]
        public int CartID { get; set; }
        public virtual Cart Cart { get; set; }

        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
