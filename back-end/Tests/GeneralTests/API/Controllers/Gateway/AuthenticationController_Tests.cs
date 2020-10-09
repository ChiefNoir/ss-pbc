using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Model;
using GeneralTests.SharedUtils;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Security;
using System;
using System.Collections;
using System.Collections.Generic;
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

        class InvalidTokens : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    string.Empty
                };
                yield return new object[]
                {
                    null
                };
                yield return new object[]
                {
                    "bad-token"
                };
                yield return new object[]
                {
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2EiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTYwMTk5Njk3MSwiZXhwIjoxNjAxOTk4NzcxLCJpc3MiOiJJc3N1ZXJOYW1lIiwiYXVkIjoiQXVkaWVuY2UtMSJ9.DCbppW8SqvL1QJS2BIO2qlplZv-UHqI2_NP_Za0KDzA"
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
        internal async void LoginAsync_Valid_Empty(string login, string password)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateAuthenticationController(context);

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
        internal async void LoginAsync_Invalid(Credentials credentials)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateAuthenticationController(context);

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

        [Theory]
        [InlineData("sa", "sa")]
        internal async void Validate_Valid(string login, string password)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateAuthenticationController(context);

                    var authResponse =
                    (
                        await api.LoginAsync(new Credentials { Login = login, Password = password }) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckSucceed(authResponse);
                   
                    var response =
                    (
                        await api.ValidateAsync(authResponse.Data.Token) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckSucceed(response);
                    Compare(authResponse.Data.Account, response.Data.Account);
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
        [InlineData("sa", "sa")]
        internal async void Validate_Valid_DeadAccount(string login, string password)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateAuthenticationController(context);

                    var authResponse =
                    (
                        await api.LoginAsync(new Credentials { Login = login, Password = password }) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckSucceed(authResponse);

                    //delete accounts from db
                    Storage.RunSql("delete from account");

                    var response =
                    (
                        await api.ValidateAsync(authResponse.Data.Token) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckFail(response);
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
        [ClassData(typeof(InvalidTokens))]
        internal async void Validate_InValid(string token)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreateAuthenticationController(context);

                    var response =
                    (
                       await api.ValidateAsync(token) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckFail(response);
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
