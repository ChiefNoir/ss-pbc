using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.System;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests.API.Controllers.Gateway
{
    public class AuthenticationController_Tests
    {
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

        private readonly Account DefaultAccount = new Account
        {
            Id = 1,
            Login = "sa",
            Password = "sa",
            Role = RoleNames.Admin,
            Version = 0
        };


        [Theory]
        [InlineData("sa", "sa")]
        internal async Task LoginAsync_Valid_EmptyAsync(string login, string password)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateGatewayController(context);

                    var response =
                    (
                        await api.LoginAsync(new Credentials { Login = login, Password = password }) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckSucceed(response);

                    Assert.NotNull(response.Data.Account);
                    Assert.NotNull(response.Data.Token);
                    Assert.Equal
                    (
                        Storage.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        response.Data.TokenLifeTimeMinutes
                    );
                    Compare(DefaultAccount, response.Data.Account);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidLogins))]
        internal async Task LoginAsync_InvalidAsync(Credentials credentials)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateGatewayController(context);

                    var response =
                    (
                        await api.LoginAsync(credentials) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckFail(response);

                    Assert.Null(response.Data);
                    Assert.Null(response.Data?.Token);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        private static void Compare(Account expected, Account actual)
        {
            Assert.Equal(expected.Login, actual.Login);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.Password); // Password from service will be always null
            Assert.Equal(expected.Role, actual.Role);
            Assert.Equal(expected.Version, actual.Version);
        }

    }
}
