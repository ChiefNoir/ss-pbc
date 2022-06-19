using Abstractions.Models;
using Security.Models;

namespace GeneralTests
{
    internal class Validator
    {
        internal static void CheckFail<T>(ExecutionResult<T> response)
        {
            if (typeof(T).IsArray)
            {
                var arr = response.Data as Array;
                if (arr != null)
                {
                    Assert.Empty(response.Data as Array);
                }
            }
            else if (response.Data is IEnumerable<T>)
            {
                Assert.Empty(response.Data as IEnumerable<T>);
            }
            else if (typeof(T).IsValueType)
            {
                Assert.Equal(default, response.Data);
            }
            else
            {
                Assert.Null(response.Data);
            }

            Assert.False(response.IsSucceed);
            Assert.NotNull(response.Error);
        }

        internal static void CheckSucceed<T>(ExecutionResult<T> response, bool allowDefault = false) 
            where T: notnull
        {
            Assert.NotNull(response);
            Assert.True(response!.IsSucceed);
            Assert.Null(response!.Error);

            if (allowDefault)
            {
                Assert.Equal(default, response!.Data);
            }
            else
            {
                Assert.NotEqual(default, response!.Data);
            }
        }

        internal static void Compare(Category expected, Category actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.DisplayName, actual.DisplayName);
            Assert.Equal(expected.IsEverything, actual.IsEverything);
            Assert.Equal(expected.TotalProjects, actual.TotalProjects);
            Assert.Equal(expected.Version, actual.Version);
        }

        internal static void Compare(Introduction expected, Introduction actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.Version, actual.Version);

            Assert.Equal(expected.ExternalUrls.Count(), actual.ExternalUrls.Count());

            foreach (var expectedUrl in expected.ExternalUrls)
            {
                var actualUrl = actual.ExternalUrls.First(x => x.DisplayName == expectedUrl.DisplayName);

                Assert.Equal(expectedUrl.DisplayName, actualUrl.DisplayName);
                Assert.Equal(expectedUrl.Url, actualUrl.Url);
                Assert.Equal(expectedUrl.Version, actualUrl.Version);
            }
        }

        internal static void CompareOpposite(Introduction expected, Introduction actual)
        {
            Assert.NotEqual(expected.Title, actual.Title);
            Assert.NotEqual(expected.Content, actual.Content);
            Assert.NotEqual(expected.PosterUrl, actual.PosterUrl);
            Assert.NotEqual(expected.PosterDescription, actual.PosterDescription);
            Assert.NotEqual(expected.Version, actual.Version);

            foreach (var expectedUrl in expected.ExternalUrls)
            {
                var actualUrl = actual.ExternalUrls.First(x => x.DisplayName == expectedUrl.DisplayName);

                Assert.NotEqual(expectedUrl.DisplayName, actualUrl.DisplayName);
                Assert.NotEqual(expectedUrl.Url, actualUrl.Url);
                Assert.NotEqual(expectedUrl.Version, actualUrl.Version);
            }
        }


        internal static void Compare(ProjectPreview expected, ProjectPreview actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.DisplayName, actual.DisplayName);

            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);

            Compare(expected.Category, actual.Category);
        }

        internal static void Compare(IEnumerable<ProjectPreview> expected, IEnumerable<ProjectPreview> actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            foreach (var item in expected)
            {
                var act = actual.First(x => x.Code == item.Code);

                Compare(item, act);
            }
        }

        internal static void Compare(IEnumerable<Category> expected, IEnumerable<Category> actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            foreach (var item in expected)
            {
                var act = actual.First(x => x.Code == item.Code);

                Compare(item, act);
            }
        }

        internal static void Compare(Account expected, Account actual)
        {
            Assert.Equal(expected.Login, actual.Login);
            Assert.Equal(expected.Role, actual.Role);
            Assert.Null(actual.Password);
            Assert.Equal(expected.Version, actual.Version);
        }
    }
}
