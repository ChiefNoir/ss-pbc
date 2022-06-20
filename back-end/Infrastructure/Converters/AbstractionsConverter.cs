using Infrastructure.Helpers;
using Infrastructure.Models;
using Security.Models;

namespace Infrastructure.Converters
{
    internal static class AbstractionsConverter
    {
        internal static Account ToAccount(Abstractions.Models.Account account, HashResult hashedPassword)
        {
            return new Account
            {
                Id = account.Id == null ? Guid.NewGuid() : account.Id,
                Login = account.Login,
                Password = hashedPassword.HexHash,
                Salt = hashedPassword.HexSalt,
                Role = account.Role
            };
        }

        internal static Category ToCategory(Abstractions.Models.Category category)
        {
            return new Category
            {
                Code = Sanitizer.SanitizeCode(category.Code),
                DisplayName = category.DisplayName,
                Version = 0
            };
        }


        internal static IntroductionToExternalUrl ToIntroductionExternalUrl(Abstractions.Models.ExternalUrl item)
        {
            var ext = ToExternalUrl(item);

            return new IntroductionToExternalUrl
            {
                ExternalUrl = ext,
                ExternalUrlId = ext!.Id!.Value,
            };
        }

        internal static Project ToProject(Abstractions.Models.Project project)
        {
            var dbProject = new Project
            {
                Id = project.Id ?? Guid.NewGuid(),
                Code = Sanitizer.SanitizeCode(project.Code),
                Description = project.Description,
                DescriptionShort = project.DescriptionShort,
                DisplayName = project.DisplayName,
                PosterDescription = project.PosterDescription,
                PosterUrl = project.PosterUrl,
                ReleaseDate = project.ReleaseDate == null ? null : DateTime.SpecifyKind(project.ReleaseDate.Value, DateTimeKind.Utc),
                Version = project.Version,
                CategoryId = project.Category.Id.Value,
                ExternalUrls = new List<ProjectToExternalUrl>()
            };

            foreach (var item in ToProjectExternalUrls(project.ExternalUrls))
            {
                item.Project = dbProject;
                item.ProjectId = dbProject.Id.Value;

                dbProject.ExternalUrls.Add(item);
            }

            return dbProject;
        }

        internal static ProjectToExternalUrl ToProjectExternalUrl(Abstractions.Models.ExternalUrl externalUrl)
        {
            return new ProjectToExternalUrl
            {
                ExternalUrl = ToExternalUrl(externalUrl),
                ExternalUrlId = externalUrl.Id ?? Guid.NewGuid()
            };
        }

        private static ExternalUrl ToExternalUrl(Abstractions.Models.ExternalUrl externalUrl)
        {
            return new ExternalUrl
            {
                Id = externalUrl.Id ?? Guid.NewGuid(),
                DisplayName = externalUrl.DisplayName,
                Url = externalUrl.Url,
                Version = externalUrl.Version
            };
        }

        private static IEnumerable<ProjectToExternalUrl> ToProjectExternalUrls(IEnumerable<Abstractions.Models.ExternalUrl> externalUrls)
        {
            if (externalUrls == null)
            {
                return new List<ProjectToExternalUrl>();
            }

            return externalUrls.Select(ToProjectExternalUrl);
        }
    }
}
