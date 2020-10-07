using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Public;
using GeneralTests.Utils;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicIntroductionController_Tests
    {
        class Defaults : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Introduction
                    {
                        Title = "Hello",
                        Content = "The service is on-line. Congratulations.",
                        PosterDescription = null,
                        PosterUrl = null,
                        Version = 0,
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

            }
        }

        private static PublicIntroductionController CreatePublicIntroductionController(DataContext context)
        {
            var config = Storage.InitConfiguration();
            var introductionRep = new IntroductionRepository(context);
            var tokenManager = new TokenManager(config);
            var sup = new Supervisor(tokenManager);

            return new PublicIntroductionController(introductionRep, sup);
        }

        [Theory]
        [ClassData(typeof(Defaults))]
        internal async void GetIntroduction_Test(Introduction expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = CreatePublicIntroductionController(context);
                    var response =
                    (
                        await api.GetIntroduction() as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckValid(response);
                    Compare(expected, response.Data);
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

        private void Compare(Introduction expected, Introduction actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.Version, actual.Version);

            Assert.Equal(expected.ExternalUrls.Count(), actual.ExternalUrls.Count());

            foreach (var expectedUrl in expected.ExternalUrls)
            {
                var actualUrl = actual.ExternalUrls.FirstOrDefault(x => x.DisplayName == expectedUrl.DisplayName);

                Assert.Equal(expectedUrl.DisplayName, actualUrl.DisplayName);
                Assert.Equal(expectedUrl.Url, actualUrl.Url);
                Assert.Equal(expectedUrl.Version, actualUrl.Version);
            }

        }
    }
}
