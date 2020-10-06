using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Controllers.Private;
using API.Controllers.Public;
using API.Model;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
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
        internal async void UpdateIntroduction_Test_Valid(Introduction update, Introduction expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var fileRep = new FileRepository(confing);
                    var introductionRep = new IntroductionRepository(context);
                    var logRep = new LogRepository(confing);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateIntroductionController(fileRep, confing, introductionRep, sup);
                    var apiPublic = new PublicIntroductionController(introductionRep, sup);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);

                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var updateResponse =
                    (
                        await api.SaveAsync(identity.Data.Token, update) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    Assert.NotNull(updateResponse.Data);
                    Assert.Null(updateResponse.Error);
                    Assert.True(updateResponse.IsSucceed);
                    Compare(updateResponse.Data, expected);

                    var getResponse =
                    (
                        await apiPublic.GetIntroduction() as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    Assert.NotNull(getResponse.Data);
                    Assert.Null(getResponse.Error);
                    Assert.True(getResponse.IsSucceed);
                    Compare(getResponse.Data, expected);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Theory]
        [ClassData(typeof(GenerateInValidSave))]
        internal async void UpdateIntroduction_Test_InValid(Introduction update)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var fileRep = new FileRepository(confing);
                    var introductionRep = new IntroductionRepository(context);
                    var logRep = new LogRepository(confing);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateIntroductionController(fileRep, confing, introductionRep, sup);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);

                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var updateResponse =
                    (
                        await api.SaveAsync(identity.Data.Token, update) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    Assert.Null(updateResponse.Data);
                    Assert.NotNull(updateResponse.Error);
                    Assert.False(updateResponse.IsSucceed);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Theory]
        [InlineData("token-toke-token")]
        [InlineData(null)]
        internal async void UpdateIntroduction_Test_BadToken(string token)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var fileRep = new FileRepository(confing);
                    var introductionRep = new IntroductionRepository(context);
                    var logRep = new LogRepository(confing);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateIntroductionController(fileRep, confing, introductionRep, sup);

                    var introduction = new Introduction
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
                    };

                    var updateResponse =
                    (
                        await api.SaveAsync(token, introduction) as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    Assert.Null(updateResponse.Data);
                    Assert.NotNull(updateResponse.Error);
                    Assert.False(updateResponse.IsSucceed);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
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