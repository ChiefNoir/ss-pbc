using Abstractions.IRepository;
using Abstractions.MemoryCache;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IMemoryCache<string, Category> _cache;

        public CategoryRepository(DataContext context, IMemoryCache<string, Category> cache)
        {
            _context = context;
            _cache = cache;
        }

        public Task<Category[]> GetCategories()
        {
            return Task.Run(() => { return _cache.GetAll(LoadAllCategories); });
        }

        public async Task<bool> CheckIsEverything(string code)
        {
            var item = await _cache.GetOrCreateAsync(code, () => { return GetCategory(code); });

            return item?.IsEverything == true;
        }

        private Task<Category[]> LoadAllCategories()
        {
            return _context.CategoriesWithTotalProjects
                           .AsNoTracking()
                           .Select(x => new Category
                           {
                               Code = x.Code,
                               DisplayName = x.DisplayName,
                               IsEverything = x.IsEverything,
                               TotalProjects = x.TotalProjects,
                               Version = x.Version
                           })
                           .ToArrayAsync();
        }

        private Task<Category> GetCategory(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            return _context.CategoriesWithTotalProjects
               .AsNoTracking()
               .Where(x => x.Code == code)
               .Select(x => new Category
               {
                   Code = x.Code,
                   DisplayName = x.DisplayName,
                   IsEverything = x.IsEverything,
                   TotalProjects = x.TotalProjects,
                   Version = x.Version
               })
               .FirstOrDefaultAsync();
        }

    }
}
