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

        public async Task<Category[]> GetCategories()
        {
            return await _cache.GetAllOrCreateAsync(Dodo);
        }

        public Task<Category[]> GetCategoriesOld()
        {
            return Dodo();
        }

        public Task<Category[]> Dodo()
        {
            return _context.Categories
                           .AsNoTracking()
                           .Select(x => new Category
                           {
                               Code = x.Code,
                               DisplayName = x.DisplayName,
                               IsEverything = x.IsEverything,
                               Version = x.Version
                           })
                           .ToArrayAsync();
        }

        public bool CheckIsEverything(string code)
        {
            return _context.Categories
                            .Any(x => x.Code == code && x.IsEverything);
        }
    }
}
