using BusinessService.DataLayer.Extensions;
using BusinessService.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessService.DataLayer.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public Task<int> CountAsync<T>() where T : class
        {
            return _context.Set<T>()
                            .AsNoTracking()
                            .CountAsync();
        }

        public Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Where(predicate)
                           .AsNoTracking()
                           .CountAsync();
        }


        public Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .AsNoTracking()
                           .FirstOrDefaultAsync(predicate);
        }

        public Task<T> FirstOrDefaultAsync<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .AsNoTracking()
                           .FirstOrDefaultAsync();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .AsNoTracking()
                           .FirstOrDefault(predicate);
        }

        public T FirstOrDefault<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .AsNoTracking()
                           .FirstOrDefault();
        }


        public Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Where(predicate)
                           .AsNoTracking()
                           .ToListAsync();
        }

        public Task<List<T>> GetAsync<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .AsNoTracking()
                           .ToListAsync();
        }

        public Task<List<T>> GetAsync<T>(int start, int length, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Skip(start)
                           .Take(length)
                           .AsNoTracking()
                           .ToListAsync();
        }

        public Task<List<T>> GetAsync<T>(int start, int length, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Where(predicate)
                           .Skip(start)
                           .Take(length)
                           .AsNoTracking()
                           .ToListAsync();
        }

    }
}
