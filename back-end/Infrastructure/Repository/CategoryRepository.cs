using Abstractions.Exceptions;
using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
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

        public async Task<Category> GetAsync(int id)
        {
            var result = await _context.CategoriesWithTotalProjects
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, "")
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

        public async Task<Category> GetTechnicalAsync()
        {
            var result = await _context.CategoriesWithTotalProjects
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.IsEverything);
            if (result == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, "")
                    );
            }

            return DataConverter.ToCategory(result);
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

            var dbItem = AbstractionsConverter.ToCategory(category);

            await _context.Categories.AddAsync(dbItem);

            await _context.SaveChangesAsync();

            var categoryWithProject = await _context.CategoriesWithTotalProjects.FirstOrDefaultAsync(x => x.Id == dbItem.Id);
            return DataConverter.ToCategory(categoryWithProject);
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

            var categoryWithProject = await _context.CategoriesWithTotalProjects.FirstOrDefaultAsync(x => x.Id == dbItem.Id);
            return DataConverter.ToCategory(categoryWithProject);
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
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(category.DisplayName))
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, nameof(category))
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
                        Resources.TextMessages.CantDeleteSystemCategory
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