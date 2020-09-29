using GeneralTests.Common;
using Infrastructure.DataModel;
using Infrastructure.Repository;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace GeneralTests.Functions.Infrastructure.Repository
{
    public class IntroductionRepository_Tests
    {
        class GenerateValidGet : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Introduction { },
                    new Abstractions.Model.Introduction { ExternalUrls = new List<Abstractions.Model.ExternalUrl>() }
                };

                yield return new object[]
                {
                    new Introduction
                    {
                        Content = "GenerateValidGet Content",
                        Title = "GenerateValidGet Title",
                        PosterDescription = "GenerateValidGet Poster",
                        PosterUrl = "GenerateValidGet URL",
                        Version = 92,
                        ExternalUrls = new List<IntroductionExternalUrl>()
                        {
                            new IntroductionExternalUrl
                            {
                                 ExternalUrl = new ExternalUrl
                                 {
                                     Id = 1,
                                     DisplayName = "DisplayName 0",
                                     Url = "URL 0 ",
                                     Version = 0
                                 }
                            }
                        }
                    },
                    new Abstractions.Model.Introduction 
                    {
                        Content = "GenerateValidGet Content",
                        Title = "GenerateValidGet Title",
                        PosterDescription = "GenerateValidGet Poster",
                        PosterUrl = "GenerateValidGet URL",
                        Version = 92,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>
                        {
                            new Abstractions.Model.ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "DisplayName 0",
                                Url = "URL 0 ",
                                Version = 0
                            }
                        }
                    }
                };
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidGet))]
        internal async void GetAsync_Valid(Introduction initialItem, Abstractions.Model.Introduction expected)
        {
            using(var context = Utils.CreateContext(initialItem))
            {
                var rep = new IntroductionRepository(context);
                var result = await rep.GetAsync();

                Assert.True(result.Equals(expected));

                context.Database.EnsureDeleted();
            }
        }

    }
}
