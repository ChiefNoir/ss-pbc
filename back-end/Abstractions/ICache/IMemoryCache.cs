using System;
using System.Threading.Tasks;

namespace Abstractions.MemoryCache
{
    public interface IMemoryCache<TKey, TEntity>
    {
        Task<TEntity> GetOrCreateAsync(TKey key, Func<Task<TEntity>> createItem);
        Task<TEntity[]> GetAllOrCreateAsync(Func<Task<TEntity[]>> initAll);
        void UpdateOrCreateAsync(TKey key, Func<Task<TEntity>> createItem);
        void RemoveAsync(string key);
    }

}
