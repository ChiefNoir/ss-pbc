using Abstractions.Model;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        
        [Theory]
        [ClassData(typeof(Defaults))]
        internal async Task GetIntroduction_Test(Introduction expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);
                    var response =
                    (
                        await api.GetIntroductionAsync() as JsonResult
                    ).Value as ExecutionResult<Introduction>;

                    GenericChecks.CheckSucceed(response);
                    Compare(expected, response.Data);
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
