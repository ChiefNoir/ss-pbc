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
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    foreach (var item in (CachedItemType[])Enum.GetValues(typeof(CachedItemType)))
                    {
                        await cache.FlushAsync(item);
                    }
                }
                finally
                {
                    context.Migrator.MigrateDown(0); 
                    await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task FlushWithWrongValue_MustFail()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    int value = -90;

                    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cache.FlushAsync((CachedItemType)value));
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                    await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task FlushMustWork_Introduction()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    var intro = new Introduction { Title = "Text" };
                    var isSaved = await cache.SaveAsync(intro);
                    Assert.True(isSaved, "Cache is not working");

                    var cached = await cache.GetIntroductionAsync();
                    Validator.Compare(cached, intro);

                    await cache.FlushAsync(CachedItemType.Introduction);
                    var cachedAfterFlush = await cache.GetIntroductionAsync();
                    Assert.Null(cachedAfterFlush);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                    await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task FlushMustWork_Categories()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    var items = new List<Category>
                    {
                        new Category { Code = "Text" },
                    };

                    var isSaved = await cache.SaveAsync(items);
                    Assert.True(isSaved, "Cache is not working");

                    var cached = await cache.GetCategoriesAsync();
                    Validator.Compare(cached, items);

                    await cache.FlushAsync(CachedItemType.Categories);
                    var cachedAfterFlush = await cache.GetCategoriesAsync();
                    Assert.Null(cachedAfterFlush);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                    await cache.FlushAsync();
                }
            }

        }

        [Fact]
        internal async Task FlushMustWork_ProjectPreviews()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    var items = new List<ProjectPreview>
                    {
                        new ProjectPreview { Code = "Text", Category = new Category { Code = "Hey" } },
                    };

                    var isSaved = await cache.SaveAsync(items);
                    Assert.True(isSaved, "Cache is not working");

                    var cached = await cache.GetProjectPreviewAsync();
                    Validator.Compare(cached, items);

                    await cache.FlushAsync(CachedItemType.ProjectsPreview);
                    var cachedAfterFlush = await cache.GetProjectPreviewAsync();
                    Assert.Null(cachedAfterFlush);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                    await cache.FlushAsync();
                }
            }
        }
    }
}
