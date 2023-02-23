using Abstractions.Cache;
using Abstractions.Exceptions;
using Abstractions.IRepositories;
using Abstractions.Models;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IDataCache _cache;

        public CategoryRepository(DataContext context, IDataCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            var cacheItems = await _cache.GetCategoriesAsync();
            if (cacheItems != null)
                return cacheItems;

            var dbItems = await _context.CategoriesWithTotalProjects
                                    .AsNoTracking()
                                    .Select(x => DataConverter.ToCategory(x))
                                    .ToListAsync();

            await _cache.SaveAsync(dbItems);

            return dbItems;
        }

        public async Task<Category> GetAsync(Guid? id)
        {
            if (id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, string.Empty)
                    );
            }

            var result = await _context.CategoriesWithTotalProjects
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, string.Empty)
                    );
            }

            return DataConverter.ToCategory(result);
        }

        public async Task<Category> GetAsync(string code)
        {
            var result = await _context.CategoriesWithTotalProjects
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Code == code);
            if (result == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, code)
                    );
            }

            return DataConverter.ToCategory(result);
        }

    }
}
