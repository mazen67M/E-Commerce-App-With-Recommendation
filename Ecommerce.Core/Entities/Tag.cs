using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class Tag
    {
        public int TagID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}
