using Abstractions.Cache;
using Abstractions.Exceptions;
using Abstractions.IRepositories;
using Abstractions.Models;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;
        private readonly IDataCache _cache;
        private readonly ICategoryRepository _categoryRepository;

        public ProjectRepository(DataContext context, ICategoryRepository categoryRepository, IDataCache cache)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _cache = cache;
        }


        public async Task<Project> GetAsync(string code)
        {
            var result = await _context.Projects
                                       .AsNoTracking()
                                       .Include(x => x.Category)
                                       .Include(x => x.ExternalUrls)
                                       .ThenInclude(x => x.ExternalUrl)
                                       .FirstOrDefaultAsync(x => x.Code == code);

            if (result == null)
            {
                throw new InconsistencyException(Resources.TextMessages.ProjectDoesNotExist);
            }

            return DataConverter.ToProject(result);
        }

        public async Task<IEnumerable<ProjectPreview>> GetPreviewAsync(int start, int length, string categoryCode)
        {
            if (length < 1 || start < 0)
            {
                throw new InconsistencyException(Resources.TextMessages.WrongPagingQuery);
            }

            var isEverything = true;
            var categoryId = Guid.Empty;

            if (!string.IsNullOrEmpty(categoryCode))
            {
                var category = await _categoryRepository.GetAsync(categoryCode);
                if (category == null)
                    throw new InconsistencyException(string.Format(Resources.TextMessages.CategoryDoesNotExist, categoryCode));

                categoryId = category.Id!.Value;
                isEverything = category.IsEverything;
            }

            var cachedItems = await _cache.GetProjectPreviewAsync();
            if (cachedItems == null)
            {
                var dbItems = await _context.Projects
                                 .OrderByDescending(x => x.ReleaseDate)
                                 .Include(x => x.Category)
                                 .Select(x => DataConverter.ToProjectPreview(x))
                                 .ToArrayAsync();

                await _cache.SaveAsync(dbItems);
                cachedItems = dbItems;
            }

            return cachedItems.Where(x => isEverything || x.Category!.Id == categoryId)
                                  .OrderByDescending(x => x.ReleaseDate)
                                  .Skip(start)
                                  .Take(length)
                                  .ToList();
        }

    }
}
