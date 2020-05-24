using BusinessService.DataLayer.Extensions;
using BusinessService.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessService.DataLayer.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public int Count<T>() where T : class
        {
            return _context.Set<T>()
                            .AsNoTracking()
                            .Count();
        }

        public int Count<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Where(predicate)
                           .AsNoTracking()
                           .Count();
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


        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)                           
                           .Where(predicate)
                           .AsNoTracking()
                           .ToList();
        }

        public IEnumerable<T> Get<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .AsNoTracking()
                           .ToList();
        }

        public IEnumerable<T> Get<T>(int start, int length, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Skip(start)
                           .Take(length)
                           .AsNoTracking()
                           .ToList();
        }

        public IEnumerable<T> Get<T>(int start, int length, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return _context.Set<T>()
                           .IncludeMultiple(includes)
                           .Where(predicate)
                           .Skip(start)
                           .Take(length)
                           .AsNoTracking()
                           .ToList();
        }
    }
}
