using Abstractions.MemoryCache;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public abstract class BaseCache<TKey, TEntity> : IMemoryCache<TKey, TEntity>
    {
        protected static readonly ConcurrentDictionary<TKey, TEntity> _cache = new ConcurrentDictionary<TKey, TEntity>();
        protected static readonly SemaphoreSlim _global = new SemaphoreSlim(1, 1);
        protected static readonly ConcurrentDictionary<TKey, SemaphoreSlim> _locks = new ConcurrentDictionary<TKey, SemaphoreSlim>();

        public abstract Task<TEntity[]> GetAll(Func<Task<TEntity[]>> initAll);

        public virtual async Task<TEntity> GetOrCreateAsync(TKey key, Func<Task<TEntity>> createItem)
        {
            if (!_cache.TryGetValue(key, out TEntity entity))
            {
                var localLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

                await _global.WaitAsync();
                await localLock.WaitAsync();
                try
                {
                    if (!_cache.TryGetValue(key, out entity))
                    {
                        entity = await createItem();

                        if (entity == null)
                            return default;

                        _cache.TryAdd(key, entity);
                    }
                }
                finally
                {
                    localLock.Release();
                    _global.Release();
                }
            }

            return entity;
        }

        public virtual async void RemoveAsync(TKey key)
        {
            var localLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
            await _global.WaitAsync();
            await localLock.WaitAsync();

            try
            {
                _cache.TryRemove(key, out TEntity tmp);
            }
            finally
            {
                localLock.Release();
                _global.Release();
            }
        }

        public virtual async void UpdateOrCreateAsync(TKey key, Func<Task<TEntity>> createItem)
        {
            var localLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await _global.WaitAsync();
            await localLock.WaitAsync();

            try
            {
                var entity = await createItem();

                if (entity == null)
                    _cache.TryRemove(key, out TEntity tmp);

                _cache[key] = entity;
            }
            finally
            {
                localLock.Release();
                _global.Release();
            }
        }
    }
}