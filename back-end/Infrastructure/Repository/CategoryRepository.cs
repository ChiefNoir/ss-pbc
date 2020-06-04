using Abstractions.IRepository;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public Task<Category[]> GetCategories()
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

        public bool IsEverything(string code)
        {
            return _context.Categories
                            .Any(x => x.Code == code && x.IsEverything);
        }
    }
}
