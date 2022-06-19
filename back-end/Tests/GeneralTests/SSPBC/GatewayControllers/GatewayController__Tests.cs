using Abstractions.Models;
using Abstractions.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Security.Models;
using SSPBC.Models;
using System.Collections;

namespace GeneralTests.SSPBC.GatewayControllers
{
    [Collection("database_sensitive")]
    public class GatewayController__Tests
    {
        private readonly Account DefaultAccount = new Account
        {
            Login = "sa",
            Password = "sa",
            Role = RoleNames.Admin,
            Version = 0
        };

        [Theory]
        [InlineData("sa", "sa")]
        internal async Task LoginAsync_Valid_EmptyAsync(string login, string password)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreateGatewayController(context);

                    var response =
                    (
                        (JsonResult) await api.LoginAsync(new Credentials { Login = login, Password = password })
                    ).Value as ExecutionResult<Identity>;

                    Validator.CheckSucceed(response!);
                    Validator.Compare(DefaultAccount, response!.Data!.Account);

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

        class InvalidLogins : IEnumerable<object[]>
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
        internal async Task LoginAsync_InvalidAsync(Credentials credentials)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreateGatewayController(context);

                    var response =
                    (
                        (JsonResult) await api.LoginAsync(credentials)
                    ).Value as ExecutionResult<Identity>;

                    Validator.CheckFail(response!);

                    Assert.Null(response!.Data);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
