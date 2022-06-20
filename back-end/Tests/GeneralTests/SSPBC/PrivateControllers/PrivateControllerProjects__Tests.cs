using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PrivateControllers
{
    [Collection("database_sensitive")]
    public sealed class PrivateControllerProjects__Tests
    {
        private class ValidCreate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "description",
                        DescriptionShort = "DescriptionShort",
                        DisplayName = "DisplayName",
                        PosterDescription = "PosterDescription",
                        PosterUrl = "PosterUrl",
                        ReleaseDate = null,
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl { DisplayName = "EXT-URL-DisplayName", Url = "EXT-URL-Url"}
                        }
                    },
                    new Project
                    {
                        Code = "code",
                        Description = "description",
                        DescriptionShort = "DescriptionShort",
                        DisplayName = "DisplayName",
                        PosterDescription = "PosterDescription",
                        PosterUrl = "PosterUrl",
                        ReleaseDate = null,
                        Version = 0,
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl { DisplayName = "EXT-URL-DisplayName", Url = "EXT-URL-Url"}
                        }
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(ValidCreate))]
        internal async Task SaveProject_ValidAsync(Project newProject, Project expected)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);

                    var createCategory =
                    (
                        (JsonResult) await api.SaveCategoryAsync(new Category { Code = "code", DisplayName = "DisplayName", Version = 0 })
                    ).Value as ExecutionResult<Category>;
                    Validator.CheckSucceed(createCategory!);

                    newProject.Category = createCategory!.Data!;
                    expected.Category = createCategory!.Data!;
                    expected.Category.TotalProjects = -1; // Note: YES

                    var createProject =
                    (
                        (JsonResult)await api.SaveProjectAsync(newProject)
                    ).Value as ExecutionResult<Project>;
                    Validator.CheckSucceed(createProject!);
                    Validator.Compare(expected, createProject!.Data!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
