using Abstractions.IRepository;
using Infrastructure.Converters;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly DataContext _context;

        public ProjectRepository(DataContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        public async Task<Abstractions.Model.Project> Create(Abstractions.Model.Project project)
        {
            if (project.Id != null)
                throw new Exception("Can't create with not empty id");

            var byCode = await _context.Projects.AnyAsync(x => x.Code == project.Code);
            if (byCode)
                throw new Exception("Code duplicate"); //TODO: move to the supervision

            Project dbItem = AbstractionsConverter.ToProject(project);
            _context.Projects.Add(dbItem);

            await _context.SaveChangesAsync();
            return project;
        }

        public Task<int> Delete(Abstractions.Model.Project project)
        {
            var dbItem = _context.Projects.FirstOrDefault(x => x.Id == project.Id);

            _context.Projects.Remove(dbItem);
            return _context.SaveChangesAsync();
        }

        public async Task<Abstractions.Model.ProjectPreview[]> GetProjectsPreview(int start, int length, string categoryCode)
        {
            var category = string.IsNullOrEmpty(categoryCode) ? null : await _categoryRepository.GetCategory(categoryCode);
            var isEverything = category == null || category.IsEverything;

            return await _context.Projects
                                 .Where(x => isEverything || x.CategoryId == category.Id)
                                 .OrderByDescending(x => x.ReleaseDate)
                                 .Skip(start)
                                 .Take(length)
                                 .Include(x => x.Category)
                                 .Select(x => DataConverter.ToProjectPreview(x))
                                 .ToArrayAsync();
        }

        public async Task<Abstractions.Model.Project> Read(int id)
        {
            var result = await _context.Projects
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.ExternalUrls)
                .ThenInclude(x => x.ExternalUrl)
                .Select(x => DataConverter.ToProject(x))
                .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("Not found");

            return result;
        }

        public async Task<Abstractions.Model.Project> Read(string code)
        {
            var result = await _context.Projects
                .Where(x => x.Code == code)
                .Include(x => x.Category)
                .Include(x => x.ExternalUrls)
                .ThenInclude(x => x.ExternalUrl)
                .Select(x => DataConverter.ToProject(x))
                .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("Not found");

            return result;
        }
        
        public async Task<Abstractions.Model.Project> Update(Abstractions.Model.Project project)
        {
            var dbItem = _context.Projects
                                .Include(x => x.Category)
                                .Include(x => x.ExternalUrls)
                                .FirstOrDefault(x => x.Id == project.Id);

            if (dbItem == null)
                throw new Exception($"There is no project with Id:{project.Id}");

            Merge(dbItem, project);

            await _context.SaveChangesAsync();
            return DataConverter.ToProject(dbItem);
        }

        private void Merge(Project localProject, Abstractions.Model.Project project)
        {
            localProject.CategoryId = project.Category.Id.Value;
            localProject.Code = project.Code;
            localProject.Description = project.Description;
            localProject.DescriptionShort = project.DescriptionShort;
            localProject.DisplayName = project.DisplayName;
            localProject.PosterDescription = project.PosterDescription;
            localProject.PosterUrl = project.PosterUrl;
            localProject.ReleaseDate = project.ReleaseDate;
            localProject.Version++;

            Merge(localProject, project.ExternalUrls);
        }

        private void Merge(Project localProject, IEnumerable<Abstractions.Model.ExternalUrl> externalUrls)
        {
            foreach (var item in localProject?.ExternalUrls ?? new List<ProjectExternalUrl>())
            {
                var localExternalUrl = externalUrls?.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (localExternalUrl == null)
                {
                    _context.ExternalUrls.Remove(item.ExternalUrl);
                }
                else
                {
                    item.ExternalUrl.DisplayName = localExternalUrl.DisplayName;
                    item.ExternalUrl.Url = localExternalUrl.Url;
                    item.ExternalUrl.Version++;
                }
            }

            foreach (var item in externalUrls?.Where(x => x.Id == null) ?? new List<Abstractions.Model.ExternalUrl>())
            {
                localProject.ExternalUrls.Add(AbstractionsConverter.ToProjectExternalUrl(item));
            }
        }
    }
}