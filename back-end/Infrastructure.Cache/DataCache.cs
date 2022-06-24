using Abstractions.ICache;
using Abstractions.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Cache
{
    public class DataCache : IDataCache
    {
        /* There are 3 chock-points of operations: 
         *      1. Introduction
         *      2. Categories
         *      3. ProjectsPreview
         */

        private readonly IConnectionMultiplexer _multiplexer;
        private const string KeyIntroduction = "Data:Introduction";
        private const string KeyCategories = "Data:Categories";

        public DataCache(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public async Task<Introduction?> GetIntroductionAsync()
        {
            return await Get<Introduction>(KeyIntroduction);
        }

        public async Task<bool> SaveAsync(Introduction introduction)
        {
            var db = _multiplexer.GetDatabase();

            var jsonString = JsonSerializer.Serialize(introduction);

            return await db.StringSetAsync(KeyIntroduction, jsonString);
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
    }
}
