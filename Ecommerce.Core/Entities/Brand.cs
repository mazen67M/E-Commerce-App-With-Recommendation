using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities
{
    public class Brand
    {
        public int BrandID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
