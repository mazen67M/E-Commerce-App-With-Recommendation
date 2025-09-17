using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// Gets all root categories (categories with no parent).
        Task<IEnumerable<Category>> GetRootCategoriesAsync();

        /// Gets all subcategories for a given parent category ID.
        Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentCategoryId);

        /// Gets a category by its name.
        Task<Category?> GetByNameAsync(string name);

        /// Gets categories that contain products.
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();

    }
}
