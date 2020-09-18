using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
using Infrastructure.Validation;
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

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }



        public Task<int> CountAsync()
        {
            return _context.Categories.CountAsync();
        }


        public async Task<bool> DeleteAsync(Category category)
        {
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            ModelValidation.CheckBeforeDelete(dbCategory, category);

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

        public Task<Category> GetAsync(int id)
        {
            return FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Category> GetAsync(string code)
        {
            return FirstOrDefaultAsync(x => x.Code == code);
        }

        public Task<Category> GetTechnicalAsync()
        {
            return FirstOrDefaultAsync(x => x.IsEverything);
        }


        public Task<Category> SaveAsync(Category item)
        {
            if (item.Id == null)
                return CreateAsync(item);

            return UpdateAsync(item);
        }


        private async Task<Category> CreateAsync(Category category)
        {
            ModelValidation.CheckBeforeCreate(category, _context);

            var dbItem = new DataModel.Category
            {
                Code = category.Code,
                DisplayName = category.DisplayName,
                Version = 0
            };

            _context.Categories.Add(dbItem);

            await _context.SaveChangesAsync();
            return DataConverter.ToCategory(dbItem);
        }

        private async Task<Category> UpdateAsync(Category category)
        {
            var dbItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            ModelValidation.CheckBeforeUpdate(dbItem, category, _context);

            dbItem.Code = category.Code;
            dbItem.DisplayName = category.DisplayName;
            dbItem.IsEverything = dbItem.IsEverything;
            dbItem.Version++;

            await _context.SaveChangesAsync();

            return DataConverter.ToCategory(dbItem);
        }

        private async Task<Category> FirstOrDefaultAsync(Expression<Func<DataModel.CategoryWithTotalProjects, bool>> predicate)
        {
            var result = await _context.CategoriesWithTotalProjects
               .AsNoTracking()
               .Where(predicate)
               .Select(x => DataConverter.ToCategory(x))
               .FirstOrDefaultAsync();

            return result;
        }
       
    }
}