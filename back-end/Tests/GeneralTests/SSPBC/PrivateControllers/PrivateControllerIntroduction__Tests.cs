using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PrivateControllers
{
    [Collection("database_sensitive")]
    public sealed class PrivateControllerIntroduction__Tests
    {
        private class GenerateValidSave : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Introduction
                    {
                        Content = "Well. Congratulations.",
                        Title = "Hello?",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 0,
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl
                            {
                                Id = null,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 0
                            }
                        }
                    },
                    new Introduction
                    {
                        Content = "Well. Congratulations.",
                        Title = "Hello?",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 1,
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl
                            {
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 0
                            }
                        }
                    }
                };

                yield return new object[]
                {
                    new Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 0,
                    },
                    new Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 1,
                        ExternalUrls = new List<ExternalUrl>()
                    }
                };

                yield return new object[]
                {
                    new Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 0,
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = null,
                                DisplayName = "ExternalUrl DisplayName 0",
                                Url = "ExternalUrl Url"
                            },
                            new ExternalUrl
                            {
                                Id = null,
                                DisplayName = "Alef 0",
                                Url = "Alef 0 Url"
                            }
                        },
                    },
                    new Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 1,
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                DisplayName = "ExternalUrl DisplayName 0",
                                Url = "ExternalUrl Url",
                                Version = 0
                            },
                            new ExternalUrl
                            {
                                Id = null,
                                DisplayName = "Alef 0",
                                Url = "Alef 0 Url"
                            }
                        },
                    }
                };
            }
        }

        private class GenerateInvalidSave : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Introduction
                    {
                        Content = "Well. Congratulations.",
                        Title = "Hello?",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 10,
                    }
                };
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidSave))]
        internal async Task UpdateIntroduction_ValidAsync(Introduction update, Introduction expected)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);

                    var updateResponse =
                    (
                        (JsonResult) await api.SaveIntroductionAsync(update)
                    ).Value as ExecutionResult<Introduction>;

                    Validator.CheckSucceed(updateResponse!);
                    Validator.Compare(updateResponse!.Data!, expected);

                    var apiPublic = Initializer.CreatePublicController(context);
                    var getResponse =
                    (
                        (JsonResult) await apiPublic.GetIntroductionAsync()
                    ).Value as ExecutionResult<Introduction>;

                    Validator.CheckSucceed(getResponse!);
                    Validator.Compare(getResponse!.Data!, expected);

                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Theory]
        [ClassData(typeof(GenerateInvalidSave))]
        internal async Task UpdateIntroduction_InValidAsync(Introduction update)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);

                    var updateResponse =
                    (
                        (JsonResult)await api.SaveIntroductionAsync(update)
                    ).Value as ExecutionResult<Introduction>;

                    Validator.CheckFail(updateResponse!);

                    var apiPublic = Initializer.CreatePublicController(context);
                    var getResponse =
                    (
                        (JsonResult)await apiPublic.GetIntroductionAsync()
                    ).Value as ExecutionResult<Introduction>;

                    Validator.CheckSucceed(getResponse!);
                    Validator.CompareOpposite(getResponse!.Data!, update);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
