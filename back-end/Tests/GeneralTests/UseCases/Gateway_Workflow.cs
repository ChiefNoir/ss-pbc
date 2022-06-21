using Microsoft.Extensions.Configuration;
using SSPBC.Models;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Collection("database_sensitive")]
    [Trait("Category", "Work-flow")]
    public sealed class Gateway_Workflow
    {
        private class InvalidLogins : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                            new Credentials{ Login = "sa", Password = null }
                };
                yield return new object[]
                {
                            new Credentials{ Login = "sa", Password = string.Empty }
                };
                yield return new object[]
                {
                            new Credentials{ Login = "sa", Password = "wrong" }
                };
                yield return new object[]
                {
                            new Credentials{ Login = string.Empty, Password = "sa" }
                };
                yield return new object[]
                {
                            new Credentials{ Login = null, Password = "sa" }
                };
                yield return new object[]
                {
                            new Credentials{ Login = string.Empty, Password = "sa" }
                };
                yield return new object[]
                {
                            new Credentials{ Login = "admin", Password = "sa" }
                };
                yield return new object[]
                {
                            new Credentials{ Login = null, Password = null }
                };
                yield return new object[]
                {
                            new Credentials{ Login = string.Empty, Password = string.Empty }
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidLogins))]
        internal async Task Login_Negative(Credentials credentials)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreateGatewayController(context);

                    var response =
                    (
                        await api.LoginAsync(credentials)
                    ).Value;

                    Validator.CheckFail(response!);
                    Assert.Null(response!.Data);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task Login_Positive()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreateGatewayController(context);

                    var response =
                    (
                        await api.LoginAsync(new Credentials { Login = Default.Account.Login, Password = Default.Account.Login })
                    ).Value;

                    Validator.CheckSucceed(response!);
                    Validator.Compare(Default.Account, response!.Data!.Account);

                    Assert.Equal
                    (
                        Initializer.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        response.Data.TokenLifeTimeMinutes
                    );
                    Assert.NotNull(response.Data.Token);

                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
