using Abstractions.Security;
using Microsoft.Extensions.Configuration;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class Gateway_Workflow
    {
        private class InvalidLogins : IEnumerable<object?[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object?[]> GetEnumerator()
            {
                yield return new object?[]
                {
                    "sa", null
                };
                yield return new object?[]
                {
                    "sa",string.Empty
                };
                yield return new object?[]
                {
                    "sa", "wrong"
                };
                yield return new object?[]
                {
                    string.Empty, "sa"
                };
                yield return new object?[]
                {
                    null, "sa"
                };
                yield return new object?[]
                {
                    "admin", "sa"
                };
                yield return new object?[]
                {
                    null, null
                };
                yield return new object?[]
                {
                    string.Empty, string.Empty
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidLogins))]
        internal async Task Login_Negative(string login, string password)
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreateGatewayController(context);
                    var response =
                    (
                        await api.LoginAsync(new Credentials(login, password, Default.Credentials.Fingerprint))
                    ).Value;

                    Validator.CheckFail(response);
                    Assert.Null(response.Data);
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task Login_Positive()
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreateGatewayController(context);

                    var response =
                    (
                        await api.LoginAsync(Default.Credentials)
                    ).Value;

                    Validator.CheckSucceed(response);
                    Validator.Compare(Default.Account, response.Data);

                    Assert.Equal
                    (
                        Initializer.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        response.Data.TokenLifeTimeMinutes
                    );
                    Assert.NotNull(response.Data.Token);

                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }
    }
}
