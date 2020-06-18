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
        









        public async Task<Task> SaveCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Code))
                throw new Exception("Category code can not be null or empty");

            var dbItem = await _cache.GetOrCreateAsync(category.Code, () => { return GetCategory(x => x.Code == category.Code); });

            if (category.Id == null && dbItem != null)
                throw new Exception("Category code must be unique");

            if(category.Id == null)
            {
                _context.Categories.Add(new DataModel.Category
                {
                    Code = category.Code,
                    DisplayName = category.DisplayName
                });
            }
            else
            {
                // TODO inconsistancy
                var something = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
                something.DisplayName = category.DisplayName;
                something.Code = category.Code;
                something.Version ++;
            }


            await _context.SaveChangesAsync();
            _cache.UpdateOrCreateAsync(category.Code, () => { return GetCategory(x => x.Code == category.Code); });
            
            return Task.CompletedTask;
        }


    }
}
