using Abstractions.IRepository;
using Abstractions.MemoryCache;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMemoryCache<string, Category> _cache;
        private readonly DataContext _context;

        public CategoryRepository(DataContext context, IMemoryCache<string, Category> cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> CheckIsEverything(string code)
        {
            var item = await _cache.GetOrCreateAsync(code, () => { return GetCategory(x => x.Code == code); });

            return item?.IsEverything == true;
        }

        public async Task<Category> DeleteCategory(Category category)
        {
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            _context.Categories.Remove(dbCategory);
            _cache.RemoveAsync(dbCategory.Code);

            return null;
        }

        public Task<Category[]> GetCategories()
        {
            return Task.Run(() => { return _cache.GetAll(LoadAllCategories); });
        }
        
        public Task<Category> GetCategory(string code)
        {
            return GetCategory(x => x.Code == code);
        }

        public Task<Category> GetCategory(int id)
        {
            return GetCategory(x => x.Id == id);
        }

        public Task<Category> GetEverythingCategory()
        {
            return _cache.FindOrCreateAsync(x => x.IsEverything, () => { return GetCategory(x => x.IsEverything); });
        }

        public async Task<Category> SaveCategory(Category category)
        {
            //TODO: should create dedicated SaveNew method
            if (string.IsNullOrEmpty(category.Code))
                throw new Exception("Category code can not be null or empty");

            var dbItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            var oldCode = dbItem?.Code;

            if (category.Id == null)
            {
                dbItem = new DataModel.Category
                {
                    Code = category.Code,
                    DisplayName = category.DisplayName
                };

                _context.Categories.Add(dbItem);
            }
            else
            {
                // TODO inconsistancy
                dbItem.DisplayName = category.DisplayName;
                dbItem.Code = category.Code;
                dbItem.Version++;
            }

            await _context.SaveChangesAsync();
            _cache.UpdateOrCreateAsync(category.Code, () => { return GetCategory(x => x.Code == category.Code); });

            if (!string.IsNullOrEmpty(oldCode) && oldCode != category.Code)
                _cache.RemoveAsync(oldCode);

            return Convert(dbItem);
        }

        private Category Convert(DataModel.Category cat)
        {
            return new Category
            {
                Id = cat.Id,
                Code = cat.Code,
                DisplayName = cat.DisplayName,
                IsEverything = cat.IsEverything,
                TotalProjects = -1,
                Version = cat.Version
            };
        }

        private async Task<Category> GetCategory(Expression<Func<DataModel.CategoryWithTotalProjects, bool>> predicate)
        {
            var result = await _context.CategoriesWithTotalProjects
               .AsNoTracking()
               .Where(predicate)
               .Select(x => new Category
               {
                   Id = x.Id,
                   Code = x.Code,
                   DisplayName = x.DisplayName,
                   IsEverything = x.IsEverything,
                   TotalProjects = x.TotalProjects,
                   Version = x.Version
               })
               .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("Not found");

            return result;
        }

        private Task<Category[]> LoadAllCategories()
        {
            return _context.CategoriesWithTotalProjects
                           .AsNoTracking()
                           .Select(x => new Category
                           {
                               Id = x.Id,
                               Code = x.Code,
                               DisplayName = x.DisplayName,
                               IsEverything = x.IsEverything,
                               TotalProjects = x.TotalProjects,
                               Version = x.Version
                           })
                           .ToArrayAsync();
        }
    }
}