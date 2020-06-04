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


        public Task<int> Count()
        {
            return _context.Projects.CountAsync();
        }

        public Task<int> Count(string categoryCode)
        {
            if (string.IsNullOrEmpty(categoryCode))
                return Count();

            if (_categoryRepository.CheckIsEverything(categoryCode))
                return Count();
            
            return _context.Projects.CountAsync(x => x.CategoryCode == categoryCode);
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
                    DisplayName = x.DisplayName,
                    PosterUrl = x.PosterUrl,
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

        public Task<ProjectPreview[]> GetProjects(int start, int length, string categoryCode)
        {
            if(_categoryRepository.CheckIsEverything(categoryCode))
                return _context.Projects
                    .Include(x => x.Category)
                    .Select(x => new ProjectPreview
                    {
                        Code = x.Code,
                        Description = x.DescriptionShort,
                        DisplayName = x.DisplayName,
                        PosterUrl = x.PosterUrl,
                        ReleaseDate = x.ReleaseDate,
                        Category = new Category
                        {
                            Code = x.Category.Code,
                            DisplayName = x.Category.DisplayName,
                            IsEverything = false,
                            Version = x.Category.Version
                        }
                    }).ToArrayAsync();


            return _context.Projects
                .Where(x => x.CategoryCode == categoryCode)
                .Include(x => x.Category)
                .Select(x => new ProjectPreview
                {
                    Code = x.Code,
                    Description = x.DescriptionShort,
                    DisplayName = x.DisplayName,
                    PosterUrl = x.PosterUrl,
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
