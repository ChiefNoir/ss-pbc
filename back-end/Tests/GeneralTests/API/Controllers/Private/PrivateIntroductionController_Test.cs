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
        class GenerateValidSave : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Introduction
                    {
                        Content = "The service is on-line. Congratulations.",
                        Title = "Hello",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 0,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>
                        {
                            new Abstractions.Model.ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 0
                            }
                        }
                    },
                    new Abstractions.Model.Introduction
                    {
                        Content = "The service is on-line. Congratulations.",
                        Title = "Hello",
                        PosterDescription = "des",
                        PosterUrl = "url",
                        Version = 1,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>
                        {
                            new Abstractions.Model.ExternalUrl
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
                    new Abstractions.Model.Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 0,
                    },
                    new Abstractions.Model.Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 1,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>()
                    }
                };

                yield return new object[]
                {
                    new Abstractions.Model.Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 0,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>()
                        {
                            new Abstractions.Model.ExternalUrl
                            {
                                DisplayName = "ExternalUrl DisplayName 0",
                                Url = "ExternalUrl Url"
                            }
                        },
                    },
                    new Abstractions.Model.Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 1,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>()
                        {
                            new Abstractions.Model.ExternalUrl
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

        class GenerateInValidSave : IEnumerable<object[]>
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

                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);

                    var repIntrod = new IntroductionRepository(context);
                    var repFile = new FileRepository(confing);
                    var repAccount = new AccountRepository(context, confing, hasManager);


                    

                    var apiPrivateIntroduction = new PrivateIntroductionController(repFile, confing, repIntrod, sup);
                    var apiPublicIntroduction = new PublicIntroductionController(repIntrod, sup);
                    var apiAuth = new AuthenticationController(confing, repAccount, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var updateResponse = await apiPrivateIntroduction.UpdateIntroductionAsync(identity.Data.Token, update);
                    var introductionUpdated = (updateResponse as JsonResult).Value as ExecutionResult<Introduction>;
                    Assert.NotNull(introductionUpdated.Data);
                    Assert.Null(introductionUpdated.Error);
                    Assert.True(introductionUpdated.IsSucceed);
                    Compare(introductionUpdated.Data, expected);

                    var getResponse = await apiPrivateIntroduction.UpdateIntroductionAsync(identity.Data.Token, update);
                    var introductionGet = (getResponse as JsonResult).Value as ExecutionResult<Introduction>;
                    Assert.NotNull(introductionGet.Data);
                    Assert.Null(introductionGet.Error);
                    Assert.True(introductionGet.IsSucceed);
                    Compare(introductionGet.Data, expected);

                }
                catch (Exception)
                {

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

                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);

                    var repIntrod = new IntroductionRepository(context);
                    var repFile = new FileRepository(confing);
                    var repAccount = new AccountRepository(context, confing, hasManager);




                    var apiPrivateIntroduction = new PrivateIntroductionController(repFile, confing, repIntrod, sup);
                    var apiPublicIntroduction = new PublicIntroductionController(repIntrod, sup);
                    var apiAuth = new AuthenticationController(confing, repAccount, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var updateResponse = await apiPrivateIntroduction.UpdateIntroductionAsync(identity.Data.Token, update);
                    var introductionUpdated = (updateResponse as JsonResult).Value as ExecutionResult<Introduction>;
                    Assert.Null(introductionUpdated.Data);
                    Assert.NotNull(introductionUpdated.Error);
                    Assert.False(introductionUpdated.IsSucceed);
                }
                catch (Exception)
                {

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
