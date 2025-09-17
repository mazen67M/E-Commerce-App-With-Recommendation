using Ecommerce.Core.Interfaces;
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

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

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
