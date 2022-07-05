using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class PingPong_Workflow
    {
        [Fact]
        internal async Task PingAdmin_Positive()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var apiPublic = Initializer.CreatePublicController(context, cache);
                    var publiPing =
                    (
                        apiPublic.Ping()
                    ).Value;

                    Validator.CheckSucceed(publiPing);
                    Assert.Equal("pong", publiPing.Data);
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task Ping_Positive()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var apiAdmin = Initializer.CreatePrivateController(context, cache);

                    var adminPing =
                    (
                        apiAdmin.Ping()
                    ).Value;

                    Validator.CheckSucceed(adminPing);
                    Assert.Equal("pong", adminPing.Data);
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }
    }
}
