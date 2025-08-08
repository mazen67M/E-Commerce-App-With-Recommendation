﻿using System.Linq.Expressions;

namespace Ecommerce.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate); 
        Task<IReadOnlyList<T>> ListWithIncludeAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes); }
}
