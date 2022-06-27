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

        public async Task<bool> DeleteAsync(Category category)
        {
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            CheckBeforeDelete(dbCategory, category);

            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return true;
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
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, "")
                    );
            }

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

        public async Task<Category> SaveAsync(Category item)
        {
            await _cache.FlushAsync(CachedItemType.Categories);

            if (item.Id == null)
            {
                return await CreateAsync(item);
            }

            await _cache.FlushAsync(CachedItemType.ProjectsPreview);
            return await UpdateAsync(item);
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
            if (categoryWithProject != null)
            {
                _context.Entry(categoryWithProject).Reload(); // Hack, EF returning data from cache
            }
            return DataConverter.ToCategory(categoryWithProject);
        }


        private void CheckBeforeDelete(Models.Category dbItem, Category category)
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

            var projects = _context.Projects.Any(x => x.CategoryId == category.Id);
            if (projects)
            {
                throw new InconsistencyException
                    (
                        string.Format
                        (
                            Resources.TextMessages.CantDeleteNotEmptyCategory,
                            dbItem.DisplayName
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

        private void CheckBeforeUpdate(Models.Category dbItem, Category category)
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
