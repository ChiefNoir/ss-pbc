using Infrastructure.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Converters
{
    static class AbstractionsConverter
    {
        public static Project ToProject(Abstractions.Model.Project project)
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
                 
                Category = ToCategory(project.Category)
            };

            foreach (var item in ToProjectExternalUrls(project.ExternalUrls))
            {
                item.Project = dbProject;
                item.ProjectId = dbProject.Id;

                dbProject.ExternalUrls.Add(item);
            }


            return dbProject;
        }

        private static Category ToCategory(Abstractions.Model.Category category)
        {
            return new Category
            {
                Code = category.Code,
                DisplayName = category.DisplayName,
                Version = category.Version,
                Id = category.Id ?? 0
            };
        }

        private static IEnumerable<ProjectExternalUrl> ToProjectExternalUrls(IEnumerable<Abstractions.Model.ExternalUrl> externalUrls)
        {
            return externalUrls.Select(x => ToProjectExternalUrl(x));
        }

        public static ProjectExternalUrl ToProjectExternalUrl(Abstractions.Model.ExternalUrl externalUrl)
        {
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

        public static Introduction ToIntroduction(Abstractions.Model.Introduction introduction)
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

        private static IEnumerable<IntroductionExternalUrl> ToExternalUrls(IEnumerable<Abstractions.Model.ExternalUrl> externalUrls)
        {
            return externalUrls.Select(x=> ToIntroductionExternalUrl(x));
        }

        internal static IntroductionExternalUrl ToIntroductionExternalUrl(Abstractions.Model.ExternalUrl item)
        {
            return new IntroductionExternalUrl
            {
                 ExternalUrl = ToExternalUrl(item),
                 ExternalUrlId = item.Id ?? 0
            };
        }

    }
}
