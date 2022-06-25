using Abstractions.ICache;
using Abstractions.Models;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Cache
{
    public class DataCache : IDataCache
    {
        /* There are 3 chock points: 
         *      1. Introduction
         *      2. Categories
         *      3. ProjectsPreview
         */

        private readonly IConnectionMultiplexer _multiplexer;

        private const string KeyIntroduction = "Data:Introduction";
        private const string KeyCategories = "Data:Categories";
        private const string KeyProjectPreview = "Data:ProjectPreview";

        public DataCache(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public async Task<Introduction?> GetIntroductionAsync()
        {
            return await Get<Introduction>(KeyIntroduction);
        }

        public async Task<IEnumerable<Category>?> GetCategoriesAsync()
        {
            return await GetCollection<Category>(KeyCategories);
        }

        public async Task<IEnumerable<ProjectPreview>?> GetProjectsPreviewAsync()
        {
            return await GetCollection<ProjectPreview>(KeyProjectPreview);
        }

        public async Task<bool> SaveAsync(Introduction introduction)
        {
            return await Save(KeyIntroduction, introduction);
        }

        public async Task<bool> SaveAsync(IEnumerable<Category> categories)
        {
            return await Save(KeyCategories, categories);
        }

        public async Task<bool> SaveAsync(IEnumerable<ProjectPreview> projectsPreview)
        {
            return await Save(KeyProjectPreview, projectsPreview);
        }

        public async Task FlushAsync()
        {
            var db = _multiplexer.GetDatabase();

            await db.KeyDeleteAsync(KeyIntroduction);
            await db.KeyDeleteAsync(KeyCategories);
            await db.KeyDeleteAsync(KeyProjectPreview);
        }



        private async Task<IEnumerable<T>?> GetCollection<T>(string key) where T : class
        {
            var db = _multiplexer.GetDatabase();
            var result = await db.StringGetAsync(key);

            if (result.IsNull)
                return null;

            return JsonSerializer.Deserialize<T[]>(result);
        }

        private async Task<T?> Get<T>(string key) where T : class
        {
            var db = _multiplexer.GetDatabase();
            var result = await db.StringGetAsync(key);

            if (result.IsNull)
                return null;

            return JsonSerializer.Deserialize<T>(result);
        }

        private async Task<bool> Save<T>(string key, T item)
        {
            var db = _multiplexer.GetDatabase();

            var jsonString = JsonSerializer.Serialize(item);

            return await db.StringSetAsync(key, jsonString);
        }
    }
}
