using Abstractions.Cache;
using Abstractions.Models;

namespace GeneralTests.Infrastructure.Cache
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class Flush_Tests
    {
        [Fact]
        internal async Task FlushByEnum_MustNotFail()
        {
            var cache = Initializer.CreateCache();

            foreach (var item in (CachedItemType[])Enum.GetValues(typeof(CachedItemType)))
            {
                await cache.FlushAsync(item);          
            }

            await cache.FlushAsync();
        }

        [Fact]
        internal async Task FlushWithWrongValue_MustFail()
        {
            var cache = Initializer.CreateCache();
            int value = -90;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cache.FlushAsync((CachedItemType)value));

            await cache.FlushAsync();
        }

        [Fact]
        internal async Task FlushMustWork_Introduction()
        {
            var cache = Initializer.CreateCache();

            var intro = new Introduction { Title = "Text" };
            await cache.SaveAsync(intro);

            var cached = await cache.GetIntroductionAsync();
            Validator.Compare(cached, intro);

            await cache.FlushAsync(CachedItemType.Introduction);
            var cachedAfterFlush = await cache.GetIntroductionAsync();
            Assert.Null(cachedAfterFlush);

            await cache.FlushAsync();
        }

        [Fact]
        internal async Task FlushMustWork_Categories()
        {
            var cache = Initializer.CreateCache();

            var items = new List<Category>
            {
               new Category { Code = "Text" },
            };

            await cache.SaveAsync(items);

            var cached = await cache.GetCategoriesAsync();
            Validator.Compare(cached, items);

            await cache.FlushAsync(CachedItemType.Categories);
            var cachedAfterFlush = await cache.GetCategoriesAsync();
            Assert.Null(cachedAfterFlush);

            await cache.FlushAsync();
        }

        [Fact]
        internal async Task FlushMustWork_ProjectPreviews()
        {
            var cache = Initializer.CreateCache();

            var items = new List<ProjectPreview>
            {
               new ProjectPreview { Code = "Text", Category = new Category { Code = "Hey" } },
            };

            await cache.SaveAsync(items);

            var cached = await cache.GetProjectPreviewAsync();
            Validator.Compare(cached, items);

            await cache.FlushAsync(CachedItemType.ProjectsPreview);
            var cachedAfterFlush = await cache.GetProjectPreviewAsync();
            Assert.Null(cachedAfterFlush);

            await cache.FlushAsync();
        }
    }
}
