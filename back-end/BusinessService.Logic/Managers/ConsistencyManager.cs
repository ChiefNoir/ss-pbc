using BusinessService.Common.Interfaces;
using BusinessService.Logic.Exceptions;
using BusinessService.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BusinessService.Logic.Managers
{
    public class ConsistencyManager : IConsistencyManager
    {
        private readonly DbContext _context;

        public ConsistencyManager(DbContext context)
        {
            _context = context;
        }

        public void ValidateUpdateAction<T>(object key, T item) where T : class, IVersion
        {
            var dbItem = _context.Find<T>(key);
            _context.Entry(dbItem).State = EntityState.Detached;

            if (dbItem == null)
                throw new KeyNotFoundException($"{typeof(T)} not found by key {key}.");

            if (dbItem.Version != item.Version)
                throw new InconsistencyException($"Version inconsistency in {typeof(T)}. {dbItem.Version} != {item.Version}.");
        }
    }
}
