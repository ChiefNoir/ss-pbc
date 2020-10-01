using Abstractions.Exceptions;
using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
using Infrastructure.Validation;
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

        public async Task<bool> DeleteAsync(Category category)
        {
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            CheckBeforeDelete(dbCategory, category);

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
            CheckBeforeCreate(category);

            var dbItem = new DataModel.Category
            {
                Code = category.Code,
                DisplayName = category.DisplayName,
                Version = 0
            };

            await _context.Categories.AddAsync(dbItem);

            await _context.SaveChangesAsync();
            return DataConverter.ToCategory(dbItem);
        }

        private async Task<Category> UpdateAsync(Category category)
        {
            var dbItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            CheckBeforeUpdate(dbItem, category);
            
            
            dbItem.Code = category.Code;
            dbItem.DisplayName = category.DisplayName;
            dbItem.IsEverything = category.IsEverything;
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





        private void CheckBeforeDelete(DataModel.Category dbItem, Category category)
        {
            if (category.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, category.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, category.GetType().Name)
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, category.GetType().Name)
                    );
            }

            if (dbItem.IsEverything)
            {
                throw new InconsistencyException(Resources.TextMessages.CantDeleteSystemCategory);
            }

            var catWithProjects = _context.CategoriesWithTotalProjects.FirstOrDefault(x => x.Id == category.Id);
            if (catWithProjects.TotalProjects > 0)
            {
                throw new InconsistencyException
                    (
                        string.Format
                        (
                            Resources.TextMessages.CantDeleteNotEmptyCategory, 
                            catWithProjects.TotalProjects, 
                            catWithProjects.DisplayName
                        )
                    );
            }
        }

        private void CheckBeforeCreate(Category category)
        {
            if (string.IsNullOrEmpty(category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(category.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Display name")
                    );
            }

            if (category.IsEverything)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.MustBeOnlyOne, "The systems category ")
                    );
            }

            if (_context.Categories.Any(x => x.Code == category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

        }

        private void CheckBeforeUpdate(DataModel.Category dbItem, Category category)
        {
            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, category.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(category.Code))
                    );
            }

            if (string.IsNullOrEmpty(category.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Display name")
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, category.GetType().Name)
                    );
            }

            if (dbItem.Code != category.Code && _context.Categories.Any(x => x.Code == category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "The category code")
                    );
            }
            if (dbItem.IsEverything && dbItem.IsEverything != category.IsEverything)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteSystemCategory)
                    );
            }
            if (category.IsEverything && _context.Categories.Any(x => x.Id != category.Id && x.IsEverything))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.MustBeOnlyOne, "System category ")
                    );
            }
        }
    }
}