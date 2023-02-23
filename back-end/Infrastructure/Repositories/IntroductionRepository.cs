using Abstractions.Cache;
using Abstractions.IRepositories;
using Abstractions.Models;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class IntroductionRepository : IIntroductionRepository
    {
        private readonly DataContext _context;
        private readonly IDataCache _cache;

        public IntroductionRepository(DataContext context, IDataCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Introduction> GetAsync()
        {
            var cachedItem = await _cache.GetIntroductionAsync();
            if (cachedItem != null)
                return cachedItem;

            var dbItem = await _context.Introductions
                           .Include(x => x.ExternalUrls)
                           .ThenInclude(x => x.ExternalUrl)
                           .AsNoTracking()
                           .Select(x => DataConverter.ToIntroduction(x))
                           .FirstAsync();

            await _cache.SaveAsync(dbItem);
            return dbItem;
        }
    }
}
