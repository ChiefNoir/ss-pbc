using Abstractions.Cache;
using Abstractions.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Cache
{
    public class DataCache : IDataCache
    {
        private readonly IConnectionMultiplexer _multiplexer;

        private readonly string KeyIntroduction = $"Data:{CachedItemType.Introduction}";
        private readonly string KeyCategories = $"Data:{CachedItemType.Categories}";
        private readonly string KeyProjectsPreview = $"Data:{CachedItemType.ProjectsPreview}";

        public DataCache(IConnectionMultiplexer multiplexer, string prefix)
        {
            _multiplexer = multiplexer;
            KeyIntroduction = prefix + KeyIntroduction;
            KeyCategories = prefix + KeyCategories;
        }

        public async Task<Introduction?> GetIntroductionAsync()
        {
            return await Get<Introduction>(KeyIntroduction);
        }

        public async Task<IEnumerable<Category>?> GetCategoriesAsync()
        {
            return await GetCollection<Category>(KeyCategories);
        }

        public async Task<IEnumerable<ProjectPreview>?> GetProjectPreviewAsync()
        {
            return await GetCollection<ProjectPreview>(KeyProjectsPreview);
        }



        public async Task<bool> SaveAsync(Introduction item)
        {
            return await Save(KeyIntroduction, item);
        }

        public async Task<bool> SaveAsync(IEnumerable<Category> items)
        {
            return await Save(KeyCategories, items);
        }

        public async Task<bool> SaveAsync(IEnumerable<ProjectPreview> items)
        {
            return await Save(KeyProjectsPreview, items);
        }

        public async Task FlushAsync()
        {
            if (!_multiplexer.IsConnected)
                return;

            foreach (var item in (CachedItemType[])Enum.GetValues(typeof(CachedItemType)))
            {
                await FlushAsync(item);
            }
        }

        public async Task FlushAsync(CachedItemType itemType)
        {
            if (!_multiplexer.IsConnected)
                return;

            var db = _multiplexer.GetDatabase();

            switch (itemType)
            {
                case CachedItemType.Categories:
                    {
                        await db.KeyDeleteAsync(KeyCategories);
                        break;
                    }
                case CachedItemType.Introduction:
                    {
                        await db.KeyDeleteAsync(KeyIntroduction);
                        break;
                    }
                case CachedItemType.ProjectsPreview:
                    {
                        await db.KeyDeleteAsync(KeyProjectsPreview);
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                    }
            }
        }

        private async Task<IEnumerable<T>?> GetCollection<T>(string key) where T : class
        {
            if (!_multiplexer.IsConnected)
                return null;

            var db = _multiplexer.GetDatabase();
            var result = await db.StringGetAsync(key);

            if (result.IsNull)
                return null;

            return JsonSerializer.Deserialize<T[]>(result);
        }

        private async Task<T?> Get<T>(string key) where T : class
        {
            if (!_multiplexer.IsConnected)
                return null;

            var db = _multiplexer.GetDatabase();
            var result = await db.StringGetAsync(key);

            if (result.IsNull)
                return null;

            return JsonSerializer.Deserialize<T>(result);
        }

        private async Task<bool> Save<T>(string key, T item)
        {
            if (!_multiplexer.IsConnected)
                return false;

            var db = _multiplexer.GetDatabase();

            var jsonString = JsonSerializer.Serialize(item);

            return await db.StringSetAsync(key, jsonString);
        }

    }
}
