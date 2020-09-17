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



        public Task<int> CountAsync()
        {
            return _context.Categories.CountAsync();
        }


        public async Task<bool> DeleteAsync(Category category)
        {
            if (category.Id == null)
                throw new Exception($"Can't delete new category");

            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (category.Id == null)
                throw new Exception($"Can't find category with id: {category.Id}");

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
            if (string.IsNullOrEmpty(category.Code))
                throw new Exception("Category code can not be null or empty");

            var anyCode = await _context.Categories.AnyAsync(x => x.Code == category.Code);
            if(anyCode)
                throw new Exception($"Code {category.Code} duplicate");
            
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
            if (category.Id == null)
                throw new Exception("Can't update new category");

            var oldItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(oldItem == null)
                throw new Exception($"Category {category.DisplayName} was already deleted");

            var countByCode = await _context.Categories.CountAsync(x => x.Code == category.Code);

            if(countByCode > 1)
                throw new Exception($"Code {category.Code} duplicate");

            oldItem.Code = category.Code;
            oldItem.DisplayName = category.DisplayName;
            oldItem.IsEverything = oldItem.IsEverything;
            oldItem.Version++;

            await _context.SaveChangesAsync();

            return DataConverter.ToCategory(oldItem);
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