using Infrastructure.DataModel;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Converters
{
    internal static class AbstractionsConverter
    {
        internal static Account ToAccount(Abstractions.Model.Account account, Abstractions.Model.HashResult hashedPassword)
        {
            return new Account
            {
                Login = account.Login,
                Password = hashedPassword.HexHash,
                Salt = hashedPassword.HexSalt,
                Role = account.Role
            };
        }

        private static IEnumerable<GalleryImage> ToGalleryImage(IEnumerable<Abstractions.Model.GalleryImage> items)
        {
            if (items == null)
                return new List<GalleryImage>();

            return items.Select(ToGalleryImage);
        }

        internal static GalleryImage ToGalleryImage(Abstractions.Model.GalleryImage item)
        {
            return new GalleryImage
            {
                Id = item.Id ?? 0,
                ExtraUrl = item.ExtraUrl,
                ImageUrl = item.ImageUrl,
                Version = item.Version
            };
        }

        internal static Introduction ToIntroduction(Abstractions.Model.Introduction introduction)
        {
            var result = new Introduction
            {
                Title = introduction.Title,
                Content = introduction.Content,
                PosterDescription = introduction.PosterDescription,
                PosterUrl = introduction.PosterUrl,
                Version = 0
            };

            foreach (var item in ToExternalUrls(introduction.ExternalUrls))
            {
                item.Introduction = result;
                item.IntroductionId = result.Id;

                result.ExternalUrls.Add(item);
            }

            return result;
        }

        internal static IntroductionExternalUrl ToIntroductionExternalUrl(Abstractions.Model.ExternalUrl item)
        {
            return new IntroductionExternalUrl
            {
                ExternalUrl = ToExternalUrl(item),
                ExternalUrlId = item.Id ?? 0
            };
        }

        internal static Project ToProject(Abstractions.Model.Project project)
        {
            var dbProject = new Project
            {
                Id = project.Id ?? 0,
                Code = project.Code,
                Description = project.Description,
                DescriptionShort = project.DescriptionShort,
                DisplayName = project.DisplayName,
                PosterDescription = project.PosterDescription,
                PosterUrl = project.PosterUrl,
                ReleaseDate = project.ReleaseDate,
                Version = project.Version,
                CategoryId = project.Category.Id.Value,
                ExternalUrls = new List<ProjectExternalUrl>(),
                GalleryImages = new List<GalleryImage>()
            };

            foreach (var item in ToProjectExternalUrls(project.ExternalUrls))
            {
                item.Project = dbProject;
                item.ProjectId = dbProject.Id;

                dbProject.ExternalUrls.Add(item);
            }

            foreach (var item in ToGalleryImage(project.GalleryImages))
            {
                item.Project = dbProject;
                item.ProjectId = dbProject.Id;

                dbProject.GalleryImages.Add(item);
            }

            return dbProject;
        }

        internal static ProjectExternalUrl ToProjectExternalUrl(Abstractions.Model.ExternalUrl externalUrl)
        {
            if (externalUrl == null)
                return null;

            return new ProjectExternalUrl
            {
                ExternalUrl = ToExternalUrl(externalUrl),
                ExternalUrlId = externalUrl.Id ?? 0
            };
        }
        
        private static ExternalUrl ToExternalUrl(Abstractions.Model.ExternalUrl externalUrl)
        {
            return new ExternalUrl
            {
                Id = externalUrl.Id ?? 0,
                DisplayName = externalUrl.DisplayName,
                Url = externalUrl.Url,
                Version = externalUrl.Version
            };
        }

        private static IEnumerable<IntroductionExternalUrl> ToExternalUrls(IEnumerable<Abstractions.Model.ExternalUrl> externalUrls)
        {
            if (externalUrls == null)
                return new List<IntroductionExternalUrl>();

            return externalUrls.Select(ToIntroductionExternalUrl);
        }

        private static IEnumerable<ProjectExternalUrl> ToProjectExternalUrls(IEnumerable<Abstractions.Model.ExternalUrl> externalUrls)
        {
            if (externalUrls == null)
                return new List<ProjectExternalUrl>();

            return externalUrls.Select(ToProjectExternalUrl);
        }
    }
}