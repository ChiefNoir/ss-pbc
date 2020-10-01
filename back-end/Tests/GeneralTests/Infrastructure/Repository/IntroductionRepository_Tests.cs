using GeneralTests.Utils;
using Infrastructure.DataModel;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.Infrastructure.Repository
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
                    new Abstractions.Model.Introduction
                    {
                        Content = "The service is on-line. Congratulations.",
                        Title = "Hello",
                        PosterDescription = null,
                        PosterUrl = null,
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
                    }
                };
            }
        }

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
                    new Abstractions.Model.Introduction
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
                    new Abstractions.Model.Introduction
                    {
                        Content = "New Content",
                        PosterDescription = "New poster description",
                        PosterUrl = "New poster url",
                        Title = "New title",
                        Version = 0,
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>
                        {
                            new Abstractions.Model.ExternalUrl
                            {
                                Id = 1,
                                Version = 10
                            }
                        }
                    },
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
                        ExternalUrls = new List<Abstractions.Model.ExternalUrl>
                        {
                            new Abstractions.Model.ExternalUrl
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
        [ClassData(typeof(GenerateValidGet))]
        internal async void GetAsync_Valid(Abstractions.Model.Introduction expected)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new IntroductionRepository(context);
                var result = await rep.GetAsync();

                Compare(result, expected);

                context.FlushDatabase();
            }
        }

        [Theory]
        [ClassData(typeof(GenerateValidSave))]
        internal async void SaveAsync_Valid(Abstractions.Model.Introduction newItem, Abstractions.Model.Introduction expectedItem)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new IntroductionRepository(context);
                    var result = await rep.SaveAsync(newItem);

                    Compare(result, expectedItem);
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
        internal async void SaveAsync_InValid(Abstractions.Model.Introduction newItem)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new IntroductionRepository(context);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.SaveAsync(newItem));
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


        private void Compare(Abstractions.Model.Introduction result, Abstractions.Model.Introduction expectedItem)
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
