using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CFA.DataAccess.Context;
using CFA.DataAccess.Repositories.Interfaces;

namespace CFA.DataAccess.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CFADbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CFADbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<IEnumerable<T>> GetAllWithPhonesAsync()
        {
            throw new NotImplementedException("Este método debe ser implementado en un repositorio específico.");
        }

        public virtual async Task<IEnumerable<T>> GetAllWithAddressesAsync()
        {
            throw new NotImplementedException("Este método debe ser implementado en un repositorio específico.");
        }
    }
}