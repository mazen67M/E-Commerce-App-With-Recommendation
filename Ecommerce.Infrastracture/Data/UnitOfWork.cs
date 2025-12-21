using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        // Repository instances
        private IUserRepository? _users;
        private IProductRepository? _products;
        private IOrderRepository? _orders;
        private IInventoryLogRepository? _inventoryLogs;
        private ICartRepository? _carts;
        private ICategoryRepository? _categories;
        private IBrandRepository? _brands;
        private IPaymentRepository? _payments;
        private IShippingRepository? _shippings;
        private IReviewRepository? _reviews;
        private IPromoCodeRepository? _promoCodes;
        private IWishlistRepository? _wishlists;
        private IActivityLogRepository? _activityLogs;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Repository Properties
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public IInventoryLogRepository InventoryLogs => _inventoryLogs ??= new InventoryLogRepository(_context);
        public ICartRepository Carts => _carts ??= new CartRepository(_context);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public IBrandRepository Brands => _brands ??= new BrandRepository(_context);
        public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);
        public IShippingRepository Shippings => _shippings ??= new ShippingRepository(_context);
        public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);
        public IPromoCodeRepository PromoCodes => _promoCodes ??= new PromoCodeRepository(_context);
        public IWishlistRepository Wishlists => _wishlists ??= new WishlistRepository(_context);
        public IActivityLogRepository ActivityLogs => _activityLogs ??= new ActivityLogRepository(_context);

        // Done
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        // Done
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    // Ensure any pending changes are saved within the transaction
                    await SaveChangesAsync();
                    await _transaction.CommitAsync();
                }
                finally
                {
                    // Always dispose the transaction object
                    await _transaction.DisposeAsync();
                    _transaction = null; // Reset the reference
                }
            }
        }

        // Done
        public void Dispose()
        {
            // Synchronously block on the async dispose
            DisposeAsync().AsTask().Wait();
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                // If there's an unfinished transaction, roll it back
                if (_transaction != null)
                {
                    try
                    {
                        await RollbackTransactionAsync();
                    }
                    catch
                    {
                        // Ignore exceptions during rollback on dispose
                    }
                }

                // Dispose the DbContext
                if (_context is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        // Done
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }

        // Done
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    await _transaction.RollbackAsync();
                }
                finally
                {
                    // Always dispose the transaction object
                    await _transaction.DisposeAsync();
                    _transaction = null; // Reset the reference
                }
            }
        }

        // Done
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
