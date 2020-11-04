using Abstractions.Model;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                                Url = "https://github.com/ChiefNoir",
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
        internal async Task UpdateIntroduction_ValidAsync(Introduction update, Introduction expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);
                    
                    var updateResponse =
                    (
                        await api.SaveIntroductionAsync(update) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(updateResponse);
                    Compare(updateResponse.Data, expected);
                    Assert.Equal(expected.PosterUrl, updateResponse.Data.PosterUrl);


                    var apiPublic = Storage.CreatePublicController(context);
                    var getResponse =
                    (
                        await apiPublic.GetIntroductionAsync() as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(getResponse);
                    Compare(getResponse.Data, expected);
                    Assert.Equal(expected.PosterUrl, getResponse.Data.PosterUrl);

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
        internal async Task UpdateIntroduction_InvalidAsync(Introduction update)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);
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

        [Fact]
        internal async Task UpdateIntroduction_AddFile_ValidAsync()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var update = new Introduction
                    {
                        Content = "The service is on-line. Congratulations.",
                        Title = "Hello",
                        PosterDescription = "des",
                        PosterUrl = @"Files/untitled.png",
                        Version = 0
                    };

                    var stream = File.OpenRead(update.PosterUrl);
                    var ffcollection = new FormFileCollection
                    {
                        new FormFile(stream, 0, stream.Length, "introduction[posterToUpload]", "untitled.png")
                    };

                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator
                        .CreateValid(context, ffcollection);


                    var updateResponse =
                    (
                        await api.SaveIntroductionAsync(update) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(updateResponse);

                    Assert.NotNull(updateResponse.Data.PosterUrl);

                    var config = Storage.CreateConfiguration();
                    var pathStart = config.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
                        + "/" + config.GetSection("Location:FileStorage").Get<string>();

                    Assert.StartsWith(pathStart, updateResponse.Data.PosterUrl);

                    var storagePath = config.GetSection("Location:FileStorage").Get<string>();
                    var fileExists = File.Exists(Path.Combine(storagePath, Path.GetFileName(updateResponse.Data.PosterUrl)));
                    Assert.True(fileExists);

                    var apiPublic = Storage.CreatePublicController(context);
                    var getResponse =
                    (
                        await apiPublic.GetIntroductionAsync() as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(getResponse);
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
            Assert.Equal(expectedItem.Title, result.Title);
            Assert.Equal(expectedItem.Content, result.Content);
            Assert.Equal(expectedItem.PosterDescription, result.PosterDescription);

            Assert.Equal(expectedItem.Version, result.Version);

            Assert.Equal(expectedItem.ExternalUrls.Count(), result.ExternalUrls.Count());

            foreach (var item in expectedItem.ExternalUrls)
            {
                var resultItem = result.ExternalUrls.FirstOrDefault(x => x.DisplayName == item.DisplayName);
                //because it's the only property we have to distinguish urls

                Assert.Equal(resultItem.DisplayName, item.DisplayName);
                Assert.Equal(resultItem.Url, item.Url);
                Assert.Equal(resultItem.Version, item.Version);
            }
        }
    }
}