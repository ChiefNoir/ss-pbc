using Infrastructure.Models;

namespace Infrastructure.Converters
{
    internal static class DataConverter
    {
        internal static Abstractions.Models.Account ToAccount(Account item)
        {
            return new Abstractions.Models.Account
            {
                Id = item.Id,
                Login = item.Login,
                Role = item.Role,
                Password = null,
                Version = item.Version
            };
        }

        internal static Abstractions.Models.Category ToCategory(CategoryWithTotalProjects category)
        {
            return new Abstractions.Models.Category
            {
                Id = category.Id,
                Code = category.Code,
                DisplayName = category.DisplayName,
                IsEverything = category.IsEverything,
                TotalProjects = category.TotalProjects,
                Version = category.Version
            };
        }

        internal static Abstractions.Models.Category ToCategory(Category category)
        {
            return new Abstractions.Models.Category
            {
                Id = category.Id,
                Code = category.Code,
                DisplayName = category.DisplayName,
                IsEverything = category.IsEverything,
                TotalProjects = -1,
                Version = category.Version
            };
        }

        internal static Abstractions.Models.Introduction ToIntroduction(Introduction item)
        {
            return new Abstractions.Models.Introduction
            {
                Content = item.Content ?? string.Empty,
                Title = item.Title ?? string.Empty,
                PosterDescription = item.PosterDescription ?? string.Empty,
                PosterUrl = item.PosterUrl ?? string.Empty,
                Version = item.Version,
                ExternalUrls = ToExternalUrl(item.ExternalUrls)
            };
        }

        internal static Abstractions.Models.Project ToProject(Project project)
        {
            return new Abstractions.Models.Project
            {
                Id = project.Id,
                Code = project.Code,
                Description = project.Description,
                DescriptionShort = project.DescriptionShort,
                Name = project.Name,
                PosterUrl = project.PosterUrl,
                PosterDescription = project.PosterDescription,
                ReleaseDate = project.ReleaseDate,
                Version = project.Version,
                Category = ToCategory(project.Category),
                ExternalUrls = ToExternalUrl(project.ExternalUrls),
            };
        }

        internal static Abstractions.Models.ProjectPreview ToProjectPreview(Project project)
        {
            return new Abstractions.Models.ProjectPreview
            {
                Code = project.Code,
                Description = project.DescriptionShort,
                Name = project.Name,
                PosterDescription = project.PosterDescription ?? string.Empty,
                PosterUrl = project.PosterUrl,
                ReleaseDate = project.ReleaseDate,
                Category = ToCategory(project.Category)
            };
        }

        private static Abstractions.Models.ExternalUrl ToExternalUrl(ProjectToExternalUrl dbEntity)
        {
            return new Abstractions.Models.ExternalUrl
            {
                DisplayName = dbEntity.ExternalUrl.DisplayName,
                Id = dbEntity.ExternalUrl.Id,
                Url = dbEntity.ExternalUrl.Url,
                Version = dbEntity.ExternalUrl.Version
            };
        }

        private static IList<Abstractions.Models.ExternalUrl> ToExternalUrl(ICollection<ProjectToExternalUrl> externalUrls)
        {
            var result = new List<Abstractions.Models.ExternalUrl>();

            if (externalUrls == null || !externalUrls.Any())
            {
                return result;
            }

            return externalUrls.Select(ToExternalUrl).ToList();
        }

        private static IList<Abstractions.Models.ExternalUrl> ToExternalUrl(ICollection<IntroductionToExternalUrl> items)
        {
            if (items == null || !items.Any())
            {
                return new List<Abstractions.Models.ExternalUrl>();
            }

            return items.Select(ToExternalUrl).ToList();
        }

        private static Abstractions.Models.ExternalUrl ToExternalUrl(IntroductionToExternalUrl item)
        {
            return new Abstractions.Models.ExternalUrl
            {
                Id = item.ExternalUrl.Id,
                DisplayName = item.ExternalUrl.DisplayName,
                Url = item.ExternalUrl.Url,
                Version = item.ExternalUrl.Version
            };
        }

    }
}
