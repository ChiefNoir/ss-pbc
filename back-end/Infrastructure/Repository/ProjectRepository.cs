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
                .Select(x => new Project //TODO: move convert to one place
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    DescriptionShort = x.DescriptionShort,
                    DisplayName = x.DisplayName,
                    PosterUrl = x.PosterUrl,
                    PosterDescription = x.PosterDescription,
                    ReleaseDate = x.ReleaseDate,
                    Version = x.Version,
                    Category = new Category 
                    {
                        Id = x.Category.Id,
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
            var category = string.IsNullOrEmpty(categoryCode)? null : await _categoryRepository.GetCategory(categoryCode);
            var isEverything = category == null || category.IsEverything;

            return await _context.Projects
                                 .Where(x => isEverything || x.CategoryId == category.Id)
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
            var category = string.IsNullOrEmpty(categoryCode) ? null : await _categoryRepository.GetCategory(categoryCode);
            var isEverything = category == null || category.IsEverything;

            return await _context.Projects
                                 .Where(x => isEverything || x.CategoryId == category.Id)
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

        public async Task<Project> Save(Project project)
        {
            DataModel.Project dbItem = null;

            if(project.Id != null)
            {
                dbItem = await _context.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);
            }
            else
            {
                dbItem = new DataModel.Project();
                _context.Projects.Add(dbItem);
            }
                
            dbItem.CategoryId = project.Category.Id.Value;
            dbItem.Code = project.Code;
            dbItem.Description = project.Description;
            dbItem.DescriptionShort = project.DescriptionShort;
            dbItem.DisplayName = project.DisplayName;
            dbItem.ExternalUrls = project.ExternalUrls?.Select(x => Convert(x)).ToList();
            dbItem.PosterDescription = project.PosterDescription;
            dbItem.PosterUrl = project.PosterUrl;
            dbItem.ReleaseDate = project.ReleaseDate;
            dbItem.Version++;

            project.Id = dbItem.Id;
            project.Version = dbItem.Version;


            await _context.SaveChangesAsync();
            return project;
        }


        private static DataModel.ExternalUrl Convert(ExternalUrl item)
        {
            return new DataModel.ExternalUrl
            {
                DisplayName = item.DisplayName,
                Id = item.Id ?? 0,
                Url = item.Url
            };
        }

        private static Project Convert(DataModel.Project project)
        {
            return new Project
            {
                Id = project.Id,
                Code = project.Code,
                Description = project.Description,
                DescriptionShort = project.DescriptionShort,
                DisplayName = project.DisplayName,
                PosterUrl = project.PosterUrl,
                PosterDescription = project.PosterDescription,
                ReleaseDate = project.ReleaseDate,
                Version = project.Version,
                Category = new Category
                {
                    Id = project.Category.Id,
                    Code = project.Category.Code,
                    DisplayName = project.Category.DisplayName,
                    IsEverything = false,
                    Version = project.Category.Version
                },
                ExternalUrls = project.ExternalUrls?.Select(e => Convert(e))
            };
        }
    }
}
