using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        /// Gets a brand by its name.
        Task<Brand?> GetByNameAsync(string name);

        /// Searches for brands whose names contain the specified term.
        Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm);

        /// Gets brands that have at least one associated product.
        Task<IEnumerable<Brand>> GetBrandsWithProductsAsync();

        /// Gets the N most popular brands (e.g., by product count or sales).
        Task<IEnumerable<Brand>> GetPopularBrandsAsync(int count);
    }
}
