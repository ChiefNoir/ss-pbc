using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


        public async Task<bool> DeleteAsync(Project project)
        {
            var dbItem = await _context.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);

            _context.Projects.Remove(dbItem);
            var rows = await _context.SaveChangesAsync();
            return rows == 1;
        }


        public Task<Project> GetAsync(string code)
        {
            return FirstOrDefaultAsync(x => x.Code == code);
        }

        public Task<Project> GetAsync(int id)
        {
            return FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProjectPreview[]> GetPreviewAsync(int start, int length, string categoryCode)
        {
            var category = string.IsNullOrEmpty(categoryCode) ? null : await _categoryRepository.GetAsync(categoryCode);
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


        public Task<Project> SaveAsync(Project project)
        {
            if (project.Id == null)
            {
                return CreateAsync(project);
            }
            else
            {
                return UpdateAsync(project);
            }
        }

        
        
        private async Task<Project> CreateAsync(Project project)
        {
            var byCode = await _context.Projects.AnyAsync(x => x.Code == project.Code);
            if (byCode)
                throw new Exception("Code duplicate"); //TODO: move to the supervision

            var dbItem = AbstractionsConverter.ToProject(project);
            _context.Projects.Add(dbItem);

            await _context.SaveChangesAsync();
            return project;
        }

        private async Task<Project> UpdateAsync(Project project)
        {
            var dbItem = _context.Projects
                                .Include(x => x.Category)
                                .Include(x => x.GalleryImages)
                                .Include(x => x.ExternalUrls)
                                .ThenInclude(x => x.ExternalUrl)
                                .FirstOrDefault(x => x.Id == project.Id);

            if (dbItem == null)
                throw new Exception($"Can't find project with id: {project.Id}");

            Merge(dbItem, project);

            await _context.SaveChangesAsync();
            return DataConverter.ToProject(dbItem);
        }


        private void Merge(DataModel.Project dbProject, Project project)
        {
            dbProject.CategoryId = project.Category.Id.Value;
            dbProject.Code = project.Code;
            dbProject.Description = project.Description;
            dbProject.DescriptionShort = project.DescriptionShort;
            dbProject.DisplayName = project.DisplayName;
            dbProject.PosterDescription = project.PosterDescription;
            dbProject.PosterUrl = project.PosterUrl;
            dbProject.ReleaseDate = project.ReleaseDate;
            dbProject.Version++;

            Merge(dbProject, project.ExternalUrls);
            Merge(dbProject, project.GalleryImages);
        }

        private void Merge(DataModel.Project dbProject, IEnumerable<ExternalUrl> externalUrls)
        {
            foreach (var item in dbProject?.ExternalUrls ?? new List<DataModel.ProjectExternalUrl>())
            {
                var remoteItem = externalUrls?.FirstOrDefault(x => x.Id.HasValue && x.Id == item.ExternalUrlId);

                if (remoteItem == null)
                {
                    _context.ExternalUrls.Remove(item.ExternalUrl);
                }
                else
                {
                    item.ExternalUrl.DisplayName = remoteItem.DisplayName;
                    item.ExternalUrl.Url = remoteItem.Url;
                    item.ExternalUrl.Version++;
                }
            }

            foreach (var item in externalUrls?.Where(x => x.Id == null) ?? new List<ExternalUrl>())
            {
                dbProject.ExternalUrls.Add(AbstractionsConverter.ToProjectExternalUrl(item));
            }
        }

        private void Merge(DataModel.Project dbProject, IEnumerable<GalleryImage> galleryImages)
        {
            foreach (var item in dbProject?.GalleryImages ?? new List<DataModel.GalleryImage>())
            {
                var remoteItem = galleryImages?.FirstOrDefault(x => x.Id.HasValue && x.Id == item.Id);

                if (remoteItem == null)
                {
                    _context.GalleryImages.Remove(item);
                }
                else
                {
                    item.ExtraUrl = remoteItem.ExtraUrl;
                    item.ImageUrl = remoteItem.ImageUrl;
                    item.Version++;
                }
            }

            foreach (var item in galleryImages?.Where(x => x.Id == null) ?? new List<GalleryImage>())
            {
                dbProject.GalleryImages.Add(AbstractionsConverter.ToGalleryImage(item));
            }
        }


        private async Task<Project> FirstOrDefaultAsync(Expression<Func<DataModel.Project, bool>> predicate)
        {
            var result = await _context.Projects
               .AsNoTracking()
               .Where(predicate)
               .Select(x => DataConverter.ToProject(x))
               .FirstOrDefaultAsync();

            return result;
        }

    }
}