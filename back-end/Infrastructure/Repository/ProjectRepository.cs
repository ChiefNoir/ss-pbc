using Abstractions.IRepository;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public ProjectRepository(DataContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }


        public Task<Project> GetProject(string code)
        {
            return _context.Projects
                .Where(x => x.Code == code)
                .Include(x=>x.Category)
                .Include(x=>x.ExternalUrls)
                .Select(x => new Project
                {
                    Code = code,
                    Description = x.Description,
                    DescriptionShort = x.DescriptionShort,
                    DisplayName = x.DisplayName,
                    PosterUrl = x.PosterUrl,
                    PosterDescription = x.PosterDescription,
                    ReleaseDate = x.ReleaseDate,
                    Version = x.Version,
                    Category = new Category 
                    {
                        Code = x.Category.Code,
                        DisplayName = x.Category.DisplayName,
                        IsEverything = false,
                        Version = x.Category.Version
                    },
                    ExternalUrls = x.ExternalUrls.Select(e => Convert(e))
                }).FirstOrDefaultAsync();
        }

        public async Task<ProjectPreview[]> GetProjectsPreview(int start, int length, string categoryCode)
        {
            var isEverything = string.IsNullOrEmpty(categoryCode) || await _categoryRepository.CheckIsEverything(categoryCode);

            return await _context.Projects
                                 .Where(x => isEverything || x.CategoryCode == categoryCode)
                                 .OrderByDescending(x => x.ReleaseDate)
                                 .Skip(start)
                                 .Take(length)
                                 .Include(x => x.Category)
                                 .Select(x => new ProjectPreview
                                 {
                                     Code = x.Code,
                                     Description = x.DescriptionShort,
                                     DisplayName = x.DisplayName,
                                     PosterUrl = x.PosterUrl,
                                     PosterDescription = x.PosterDescription,
                                     ReleaseDate = x.ReleaseDate,
                                     Category = new Category
                                     {
                                         Code = x.Category.Code,
                                         DisplayName = x.Category.DisplayName,
                                         IsEverything = false,
                                         Version = x.Category.Version
                                     }
                                 }).ToArrayAsync();
        }

        public async Task<Project[]> GetProjects(int start, int length, string categoryCode)
        {
            var isEverything = await _categoryRepository.CheckIsEverything(categoryCode);

            return await _context.Projects
                                 .Where(x => isEverything ? true : x.CategoryCode == categoryCode)
                                 .OrderByDescending(x => x.ReleaseDate)
                                 .Skip(start)
                                 .Take(length)
                                 .Include(x => x.Category)
                                 .Select(x => new Project
                                 {
                                     Code = x.Code,
                                     DescriptionShort = x.DescriptionShort,
                                     Description = x.Description,
                                     DisplayName = x.DisplayName,
                                     PosterUrl = x.PosterUrl,
                                     PosterDescription = x.PosterDescription,
                                     ReleaseDate = x.ReleaseDate,
                                     Category = new Category
                                     {
                                         Code = x.Category.Code,
                                         DisplayName = x.Category.DisplayName,
                                         IsEverything = false,
                                         Version = x.Category.Version
                                     }
                                 }).ToArrayAsync();
        }


        private static ExternalUrl Convert(DataModel.ExternalUrl dbEntity)
        {
            return new ExternalUrl
            {
                DisplayName = dbEntity.DisplayName,
                Id = dbEntity.Id,
                Url = dbEntity.Url,
                Version = dbEntity.Version
            };
        }
    }
}
