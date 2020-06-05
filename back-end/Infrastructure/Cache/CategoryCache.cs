using Abstractions.MemoryCache;
using Abstractions.Model;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class CategoryCache : IMemoryCache<string, Category>
    {
        private static readonly ConcurrentDictionary<string, Category> _cache = new ConcurrentDictionary<string, Category>();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        private static readonly SemaphoreSlim _global = new SemaphoreSlim(1, 1);

        public async Task<Category> GetOrCreateAsync(string key, Func<Task<Category>> createItem)
        {
            if (_cache.TryGetValue(key, out Category entity))
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
                            return null;

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

        public async Task<Category[]> GetAllOrCreateAsync(Func<Task<Category[]>> initAll)
        {
            if(_cache.Any())
                return _cache.Values.ToArray();

            await _global.WaitAsync();

            try
            {
                foreach (var item in await initAll())
                {
                    _cache.TryAdd(item.Code, item);
                }
            }
            finally
            {
                _global.Release();
            }

            return _cache.Values.ToArray();
        }

        public async void UpdateOrCreateAsync(string key, Func<Task<Category>> createItem)
        {
            var localLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await _global.WaitAsync();
            await localLock.WaitAsync();

            try
            {
                var entity = await createItem();

                if (entity == null)
                    _cache.TryRemove(key, out Category tmp);

                _cache[key] = entity;
            }
            finally
            {
                localLock.Release();
                _global.Release();
            }
        }

        public async void RemoveAsync(string key)
        {
            var localLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
            await _global.WaitAsync();
            await localLock.WaitAsync();

            try
            {
                _cache.TryRemove(key, out Category tmp);
            }
            finally
            {
                localLock.Release();
                _global.Release();
            }
        }
    }
}
