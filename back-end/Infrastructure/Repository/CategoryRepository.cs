using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<bool> CheckIsEverything(string code)
        {
            var item = await GetEverythingCategory();

            return item?.Code == code;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            _context.Categories.Remove(dbCategory);
            var rows = await _context.SaveChangesAsync();


            return rows == 1;
        }

        public Task<Category[]> GetAsync()
        {
            return _context.CategoriesWithTotalProjects
                           .AsNoTracking()
                           .Select(x => DataConverter.ToCategory(x))
                           .ToArrayAsync();
        }
        
        public Task<Category> GetAsync(string code)
        {
            return GetCategory(x => x.Code == code);
        }

        public Task<Category> GetAsync(int id)
        {
            return GetCategory(x => x.Id == id);
        }

        public Task<Category> GetEverythingCategory()
        {
            return _context.CategoriesWithTotalProjects
                           .AsNoTracking()
                           .Where(x=>x.IsEverything)
                           .Select(x => DataConverter.ToCategory(x))
                           .FirstOrDefaultAsync();
        }

        public async Task<Category> AddAsync(Category category)
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
                // TODO inconsistency
                dbItem.DisplayName = category.DisplayName;
                dbItem.Code = category.Code;
                dbItem.Version++;
            }

            await _context.SaveChangesAsync();
            return DataConverter.ToCategory(dbItem);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            if (category.Id != null)
                throw new Exception("Inconsistency");

            var hasOldDb = await _context.Categories.AnyAsync(x => x.Code == category.Code);
            if(hasOldDb)
                throw new Exception("Inconsistency");

            var dbItem = new DataModel.Category
            {
                Code = category.Code,
                DisplayName = category.DisplayName
            };

            _context.Categories.Add(dbItem);
            await _context.SaveChangesAsync();

            return DataConverter.ToCategory(dbItem);
        }


        private async Task<Category> GetCategory(Expression<Func<DataModel.CategoryWithTotalProjects, bool>> predicate)
        {
            var result = await _context.CategoriesWithTotalProjects
               .AsNoTracking()
               .Where(predicate)
               .Select(x => DataConverter.ToCategory(x))
               .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("Not found");

            return result;
        }


       
    }
}