﻿using Abstractions.Exceptions;
using Abstractions.IRepositories;
using Abstractions.Models;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
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

            CheckBeforeDelete(dbItem, project);

            _context.Projects.Remove(dbItem);
            var rows = await _context.SaveChangesAsync();
            return rows == 1;
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


        public async Task<ProjectPreview[]> GetPreviewAsync(int start, int length, string categoryCode)
        {
            if (length < 1 || start < 0)
            {
                throw new InconsistencyException(Resources.TextMessages.WrongPagingQuery);
            }

            var isEverything = true;
            var categoryId = Guid.Empty;

            // TODO: Test coverage!
            if (!string.IsNullOrEmpty(categoryCode))
            {
                var category = await _categoryRepository.GetAsync(categoryCode);
                categoryId = category.Id.Value;
                isEverything = category.IsEverything;
            }

            return await _context.Projects
                                 .Where(x => isEverything || x.CategoryId == categoryId)
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

            return UpdateAsync(project);
        }



        private async Task<Project> CreateAsync(Project project)
        {
            CheckBeforeCreate(project);

            var dbItem = AbstractionsConverter.ToProject(project);
            await _context.Projects.AddAsync(dbItem);
            await _context.SaveChangesAsync();

            var result = await _context.Projects
                                       .AsNoTracking()
                                       .Include(x => x.Category)
                                       .Include(x => x.ExternalUrls)
                                       .ThenInclude(x => x.ExternalUrl)
                                       .FirstOrDefaultAsync(x => x.Id == dbItem.Id);

            if (result == null)
            {
                throw new InconsistencyException(Resources.TextMessages.ProjectDoesNotExist);
            }

            return DataConverter.ToProject(result);
        }

        private async Task<Project> UpdateAsync(Project project)
        {
            var dbItem = _context.Projects
                                 .Include(x => x.Category)
                                 .Include(x => x.ExternalUrls)
                                 .ThenInclude(x => x.ExternalUrl)
                                 .FirstOrDefault(x => x.Id == project.Id);

            CheckBeforeUpdate(dbItem, project);

            Merge(dbItem, project);

            await _context.SaveChangesAsync();

            return DataConverter.ToProject(dbItem);
        }


        private void Merge(Models.Project dbProject, Project project)
        {
            dbProject.Category = _context.Categories.FirstOrDefault(x => x.Code == project.Category.Code);

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
        }

        private void Merge(Models.Project dbProject, IEnumerable<ExternalUrl> externalUrls)
        {
            foreach (var item in dbProject.ExternalUrls ?? new List<Models.ProjectToExternalUrl>())
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

        private static void CheckBeforeDelete(Models.Project dbItem, Project project)
        {
            if (project.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, project.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, project.GetType().Name)
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

        }

        private void CheckBeforeCreate(Project project)
        {
            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(project.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "DisplayName")
                    );
            }

            if (project.Category == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Category")
                    );
            }

            if (!_context.Categories.Any(x => x.Id == project.Category.Id))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, project.Category.Code)
                    );
            }


            if (_context.Projects.Any(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

            foreach (var item in project.ExternalUrls ?? new List<ExternalUrl>())
            {
                if (string.IsNullOrEmpty(item.DisplayName))
                {
                    throw new InconsistencyException
                    (
                       string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Display name of the External URL")
                    );
                }

                if (string.IsNullOrEmpty(item.Url))
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "URL of the External URL")
                    );
                }
            }
        }

        private void CheckBeforeUpdate(Models.Project dbItem, Project project)
        {
            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, project.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Code))
                    );
            }

            if (string.IsNullOrEmpty(project.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.DisplayName))
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

            if (dbItem.Code != project.Code && _context.Projects.Any(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, nameof(project.Code))
                    );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = project.ExternalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                {
                    continue;
                }

                if (item.ExternalUrl.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, nameof(updated))
                    );
                }
            }

            foreach (var item in project.ExternalUrls ?? new List<ExternalUrl>())
            {
                if (string.IsNullOrEmpty(item.DisplayName))
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Display name of the External URL")
                    );
                }

                if (string.IsNullOrEmpty(item.Url))
                {
                    throw new InconsistencyException
                    (
                       string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "URL of the External URL")
                    );
                }
            }
        }
    }
}
