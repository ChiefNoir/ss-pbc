using Abstractions.IRepository;
using Abstractions.MemoryCache;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
            var item = await _cache.GetOrCreateAsync(code, () => { return GetCategory(x => x.Code == code); });

            return item?.IsEverything == true;
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

        private Task<Category> GetCategory(Expression<Func<DataModel.CategoryWithTotalProjects, bool>> predicate)
        {
            return _context.CategoriesWithTotalProjects
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
        }

        public Task<Category> GetEverythingCategory()
        {
            return _cache.FindOrCreateAsync(x => x.IsEverything, () => { return GetCategory(x => x.IsEverything); });
        }

        public Task<Category> GetCategory(string code)
        {
            return GetCategory(x => x.Code == code);
        }
        









        public async Task<Category> SaveCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Code))
                throw new Exception("Category code can not be null or empty");

            var dbItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (category.Id == null && dbItem != null)
                throw new Exception("Category code must be unique");

            DataModel.Category cat = null;

            if (category.Id == null)
            {
                cat = new DataModel.Category
                {
                    Code = category.Code,
                    DisplayName = category.DisplayName
                };

                _context.Categories.Add(cat);
            }
            else
            {
                // TODO inconsistancy
                cat = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
                cat.DisplayName = category.DisplayName;
                cat.Code = category.Code;
                cat.Version ++;
            }


            await _context.SaveChangesAsync();
            _cache.UpdateOrCreateAsync(category.Code, () => { return GetCategory(x => x.Code == category.Code); });

            if (dbItem.Code != category.Code)
                _cache.RemoveAsync(dbItem.Code);

            return Convert(cat);
        }

        public async Task<Category> DeleteCategory(Category category)
        {
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            _context.Categories.Remove(dbCategory);
            _cache.RemoveAsync(dbCategory.Code);

            return null;
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

    }
}
