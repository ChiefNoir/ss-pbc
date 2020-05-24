using BusinessService.Common.Interfaces;
using BusinessService.Logic.Exceptions;
using BusinessService.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Logic.Managers
{
    /// <summary> Consistency Manager will validate <see cref="IVersion"/> entity on update action
    /// If <see cref="int"/> Version of the entity is different, then the update action is invalid and must be stopped.</summary>
    public class ConsistencyManager : IConsistencyManager
    {
        private readonly DbContext _context;

        public ConsistencyManager(DbContext context)
        {
            _context = context;
        }

        public void ValidateBeforeUpdate<T>(object key, T entity) where T : class, IVersion
        {
            var dbEntity = _context.Find<T>(key);
            _context.Entry(dbEntity).State = EntityState.Detached;

            if (dbEntity == null)
                throw new NotFoundException($"{typeof(T)} not found by key {key}.");

            if (dbEntity.Version != entity.Version)
                throw new InconsistencyException($"Version inconsistency in {typeof(T)}. {dbEntity.Version} != {entity.Version}.");
        }
    }
}
