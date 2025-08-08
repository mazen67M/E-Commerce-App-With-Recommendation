using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class ProductTag
    {
        public int ProductTagID { get; set; }

        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int TagID { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
