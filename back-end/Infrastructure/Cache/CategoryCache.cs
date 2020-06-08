using Abstractions.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    /// <summary> In-memory cache for <seealso cref="Category"/> items </summary>
    public class CategoryCache : BaseCache<string, Category>
    {
        /// <summary> The <see cref="GetAll"/> initialization can run only once </summary>
        private static bool _wasInit = false;

        public override async Task<Category[]> GetAll(Func<Task<Category[]>> initAll)
        {
            if(!_wasInit)
            {
                await _global.WaitAsync();                
                try
                {
                    if (!_wasInit)
                    {
                        _cache.Clear();
                        foreach (var item in await initAll())
                        {
                            _cache.TryAdd(item.Code, item);
                        }

                        _wasInit = true;
                    }
                }
                finally
                {
                    _global.Release();
                }
            }

            return _cache.Values.ToArray();

        }

        public override async Task<Category> FindOrCreateAsync(Func<Category, bool> predicate, Func<Task<Category>> createItem)
        {
            var stored = _cache.Values.FirstOrDefault(predicate);
            if (stored != null)
                return stored;

            var entity = await createItem();
            if (entity == null)
                return null;

            var localLock = _locks.GetOrAdd(entity.Code, k => new SemaphoreSlim(1, 1));

            await _global.WaitAsync();
            await localLock.WaitAsync();
            try
            {
                _cache[entity.Code] = entity;
            }
            finally
            {
                localLock.Release();
                _global.Release();
            }

            return entity;
        }

    }
}
