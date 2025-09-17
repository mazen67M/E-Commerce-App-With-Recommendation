using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly AppDbContext _context;

        public BrandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        // Done
        public async Task<IEnumerable<Brand>> GetBrandsWithProductsAsync()
        {
            return await _context.Brands
                .Where(b=>b.Products.Any())
                .Include(b=>b.Products)
                .ToListAsync();
        }

        // Done
        public async Task<Brand?> GetByNameAsync(string name)
        {
            return await _context.Brands
                 .FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());
        }

        // Done
        public async Task<IEnumerable<Brand>> GetPopularBrandsAsync(int count)
        {
            return await _context.Products
                .Where(p=>p.BrandID.HasValue)
                .GroupBy(p=>p.BrandID)
                .Select(g=> new {
                    BrandID = g.Key, ProductCount = g.Count()})
                .OrderByDescending(g=>g.ProductCount)
                .Take(count)
                .Join(_context.Brands,
                      g => g.BrandID,
                      b => b.BrandID,
                      (g, b) => b)
                .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Brand>> SearchByNameAsync(string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
            {
                return await ListAllAsync();
            }

            return await _context.Brands
                .Where(b=>b.Name.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
