using Abstractions.Model;
using Abstractions.Model.System;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.API.Controllers.Private
{
    public class PrivateIntroductionController_Test
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
                        Content = "The service is on-line. Congratulations.",
                        Title = "Hello",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 0,
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 0
                            }
                        }
                    },
                    new Introduction
                    {
                        Content = "The service is on-line. Congratulations.",
                        Title = "Hello",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 1,
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "GitHub",
                                Url = "New value https://github.com/ChiefNoir",
                                Version = 1
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
                                DisplayName = "ExternalUrl DisplayName 0",
                                Url = "ExternalUrl Url"
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
                                Id = 1,
                                DisplayName = "ExternalUrl DisplayName 0",
                                Url = "ExternalUrl Url",
                                Version = 0
                            }
                        },
                    }
                };
            }
        }

        private class GenerateInValidSave : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 10,
                    },
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
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl
                            {
                                Id = 1,
                                Version = 10
                            }
                        }
                    },
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
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl
                            {
                                Id = 12,
                                Version = 0
                            }
                        }
                    },
                };
                yield return new object[]
                {
                    null
                };
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidSave))]
        internal async void UpdateIntroduction_Valid(Introduction update, Introduction expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    var updateResponse =
                    (
                        await api.SaveIntroductionAsync(update) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(updateResponse);
                    Compare(updateResponse.Data, expected);

                    var apiPublic = Storage.CreatePublicController(context);
                    var getResponse =
                    (
                        await apiPublic.GetIntroductionAsync() as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(getResponse);
                    Compare(getResponse.Data, expected);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(GenerateInValidSave))]
        internal async void UpdateIntroduction_Invalid(Introduction update)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    var updateResponse =
                    (
                        await api.SaveIntroductionAsync(update) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckFail(updateResponse);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        private void Compare(Introduction result, Introduction expectedItem)
        {
            Assert.Equal(result.Title, expectedItem.Title);
            Assert.Equal(result.Content, expectedItem.Content);
            Assert.Equal(result.PosterDescription, expectedItem.PosterDescription);
            Assert.Equal(result.PosterUrl, expectedItem.PosterUrl);
            Assert.Equal(result.Version, expectedItem.Version);

            Assert.Equal(result.ExternalUrls.Count(), expectedItem.ExternalUrls.Count());

            foreach (var item in expectedItem.ExternalUrls)
            {
                var resultItem = result.ExternalUrls.FirstOrDefault(x => x.DisplayName == item.DisplayName);
                //because it's the only property we have to distinguish urls

                Assert.Equal(resultItem.DisplayName, resultItem.DisplayName);
                Assert.Equal(resultItem.Url, resultItem.Url);
                Assert.Equal(resultItem.Version, resultItem.Version);
            }
        }
    }
}