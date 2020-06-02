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

        public async Task<int> CountAsync<T>() where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .CountAsync();
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .Where(predicate)                           
                                 .CountAsync();
        }


        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .FirstOrDefaultAsync();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .AsNoTracking()
                           .IncludeMultiple(includes)
                           .FirstOrDefault(predicate);
        }

        public T FirstOrDefault<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .AsNoTracking()
                           .IncludeMultiple(includes)
                           .FirstOrDefault();
        }


        public async Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .Where(predicate)
                                 .ToListAsync();
        }

        public async Task<List<T>> GetAsync<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .ToListAsync();
        }

        public async Task<List<T>> GetAsync<T>(int start, int length, params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .Skip(start)
                                 .Take(length)
                                 .ToListAsync();
        }

        public async Task<List<T>> GetAsync<T>(int start, int length, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .IncludeMultiple(includes)
                                 .Where(predicate)
                                 .Skip(start)
                                 .Take(length)
                                 .ToListAsync();
        }

    }
}
