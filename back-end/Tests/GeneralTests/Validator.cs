using Abstractions.Models;
using Security.Models;
using SSPBC.Admin.Models;
using SSPBC.Models;

namespace GeneralTests
{
    internal class Validator
    {
        internal static void CheckFail<T>(ExecutionResult<T> response)
        {
            if (typeof(T).IsValueType)
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

        internal static void CheckSucceed<T>(ExecutionResult<T> response)
            where T : notnull
        {
            Assert.NotNull(response);
            Assert.True(response.IsSucceed);
            Assert.Null(response.Error);
        }

        internal static void Compare(Category expected, Category actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.DisplayName, actual.DisplayName);
            Assert.Equal(expected.IsEverything, actual.IsEverything);
            //Assert.Equal(expected.TotalProjects, actual.TotalProjects); TODO: fix it
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

        internal static void Compare(ProjectPreview expected, ProjectPreview actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Name, actual.Name);

            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);

            Compare(expected.Category, actual.Category);
        }

        internal static void Compare(Project expected, Project actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.DescriptionShort, actual.DescriptionShort);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Version, actual.Version);

            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);

            Compare(expected.Category, actual.Category);
            foreach (var expectedUrl in expected.ExternalUrls ?? Enumerable.Empty<ExternalUrl>())
            {
                var actualUrl = actual.ExternalUrls.First(x => x.DisplayName == expectedUrl.DisplayName);

                Assert.Equal(expectedUrl.DisplayName, actualUrl.DisplayName);
                Assert.Equal(expectedUrl.Url, actualUrl.Url);
                Assert.Equal(expectedUrl.Version, actualUrl.Version);
            }
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

        internal static void Compare(IEnumerable<string> expected, IEnumerable<string> actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            foreach (var item in expected)
            {
                var act = actual.First(x => x == item);
                Assert.Equal(item, act);
            }
        }

        internal static void Compare(IEnumerable<Account> expected, IEnumerable<Account> actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            foreach (var item in expected)
            {
                var act = actual.First(x => x.Login == item.Login);
                Compare(item, act);
            }
        }

        internal static void Compare(Account account, Identity identity)
        {
            Assert.Equal(account.Login, identity.Login);
            Assert.Equal(account.Role, identity.Role);
            Assert.NotNull(identity.Token);
        }
    }
}
