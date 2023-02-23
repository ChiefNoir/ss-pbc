using Abstractions.Cache;
using Abstractions.Models;
using Infrastructure.Cache;
using StackExchange.Redis;

namespace GeneralTests.UseCases  // <-- HACK: to ensure sequential execution
{
    [Trait("Category", "e2e")]
    [Collection(nameof(NonParallelCollection))]
    public sealed class NoConnection_Tests
    {
        private static readonly string _fakeConnection = "127.0.0.1:9999,password=9999,abortConnect=false";

        [Fact]
        internal async Task NoConnection_Introduction()
        {
            IDataCache cache = new DataCache(ConnectionMultiplexer.Connect(_fakeConnection));

            var item = await cache.GetIntroductionAsync();
            Assert.Null(item);

            var saveItemResult = await cache.SaveAsync(new Introduction { Title = "None" });
            Assert.False(saveItemResult);

            var newItem = await cache.GetIntroductionAsync();
            Assert.Null(newItem);

            await cache.FlushAsync();
            await cache.FlushAsync(CachedItemType.Introduction);
        }

        [Fact]
        internal async Task NoConnection_Categories()
        {
            IDataCache cache = new DataCache(ConnectionMultiplexer.Connect(_fakeConnection));

            var items = await cache.GetCategoriesAsync();
            Assert.Null(items);

            var saveItemResult = await cache.SaveAsync(new List<Category>
            {
                new Category{ Id = Guid.NewGuid() }
            });
            Assert.False(saveItemResult);

            var newItems = await cache.GetCategoriesAsync();
            Assert.Null(newItems);

            await cache.FlushAsync();
            await cache.FlushAsync(CachedItemType.Categories);
        }

        [Fact]
        internal async Task NoConnection_ProjectsPreview()
        {
            IDataCache cache = new DataCache(ConnectionMultiplexer.Connect(_fakeConnection));

            var items = await cache.GetProjectPreviewAsync();
            Assert.Null(items);

            var saveItemResult = await cache.SaveAsync(new List<ProjectPreview>
            {
                new ProjectPreview{  Code = "No" }
            });
            Assert.False(saveItemResult);

            var newItems = await cache.GetProjectPreviewAsync();
            Assert.Null(newItems);

            await cache.FlushAsync();
            await cache.FlushAsync(CachedItemType.ProjectsPreview);
        }
    }
}
