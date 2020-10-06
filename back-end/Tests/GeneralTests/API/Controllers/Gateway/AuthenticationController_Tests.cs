using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Model;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
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
                    new Credentials{ Login = null, Password = "sa" }
                };
                yield return new object[]
                {
                    new Credentials{ Login = "sa", Password = null }
                };
                yield return new object[]
                {
                    new Credentials{ Login = "admin", Password = "sa" }
                };
                yield return new object[]
                {
                    new Credentials{ Login = "sa", Password = "wrong" }
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
                    null
                };
                yield return new object[]
                {
                    "bad-token"
                };
            }
        }

        [Fact]
        internal async void LoginAsync_Valid_Empty()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var config = Storage.InitConfiguration();
                    var hashManager = new HashManager();
                    var accRep = new AccountRepository(context, config, hashManager);
                    var log = new LogRepository(config);
                    var tokenManager = new TokenManager(config);
                    var sup = new Supervisor(log, tokenManager);


                    var api = new AuthenticationController(config, accRep, sup, tokenManager);

                    var response = await api.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var result = (response as JsonResult).Value as ExecutionResult<Identity>;

                    Assert.True(result.IsSucceed);
                    Assert.Null(result.Error);

                    Assert.Equal("sa", result.Data.Account.Login);
                    Assert.Null(result.Data.Account.Password);
                    Assert.Equal(1, result.Data.Account.Id);
                    Assert.Equal(0, result.Data.Account.Version);
                    Assert.Equal("admin", result.Data.Account.Role);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Fact]
        internal async void LoginAsync_InValid_Empty()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var config = Storage.InitConfiguration();
                    var hashManager = new HashManager();
                    var accRep = new AccountRepository(context, config, hashManager);
                    var log = new LogRepository(config);
                    var tokenManager = new TokenManager(config);
                    var sup = new Supervisor(log, tokenManager);


                    var api = new AuthenticationController(config, accRep, sup, tokenManager);

                    var response = await api.LoginAsync(new Credentials { Login = "admin", Password = "admin" });
                    var result = (response as JsonResult).Value as ExecutionResult<Identity>;

                    Assert.False(result.IsSucceed);
                    Assert.NotNull(result.Error);
                    Assert.Null(result.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidLogins))]
        internal async void LoginAsync_InValid(Credentials credentials)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var config = Storage.InitConfiguration();
                    var hashManager = new HashManager();
                    var accRep = new AccountRepository(context, config, hashManager);
                    var log = new LogRepository(config);
                    var tokenManager = new TokenManager(config);
                    var sup = new Supervisor(log, tokenManager);


                    var api = new AuthenticationController(config, accRep, sup, tokenManager);

                    var response = 
                    (
                        await api.LoginAsync(credentials) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.False(response.IsSucceed);
                    Assert.NotNull(response.Error);
                    Assert.Null(response.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Fact]
        internal async void Validate_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var config = Storage.InitConfiguration();
                    var hashManager = new HashManager();
                    var accRep = new AccountRepository(context, config, hashManager);
                    var log = new LogRepository(config);
                    var tokenManager = new TokenManager(config);
                    var sup = new Supervisor(log, tokenManager);


                    var api = new AuthenticationController(config, accRep, sup, tokenManager);

                    var authResponse =
                    (
                        await api.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                   
                    var response =
                    (
                        await api.ValidateAsync(authResponse.Data.Token) as JsonResult
                    ).Value as ExecutionResult<Identity>;


                    Assert.True(response.IsSucceed);
                    Assert.Null(response.Error);
                    Assert.NotNull(response.Data);

                    Compare(authResponse.Data.Account, response.Data.Account);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Fact]
        internal async void Validate_Valid_DeadAccount()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var config = Storage.InitConfiguration();
                    var hashManager = new HashManager();
                    var accRep = new AccountRepository(context, config, hashManager);
                    var log = new LogRepository(config);
                    var tokenManager = new TokenManager(config);
                    var sup = new Supervisor(log, tokenManager);


                    var api = new AuthenticationController(config, accRep, sup, tokenManager);

                    var authResponse =
                    (
                        await api.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Storage.RunSql("delete from account");

                    var response =
                    (
                        await api.ValidateAsync(authResponse.Data.Token) as JsonResult
                    ).Value as ExecutionResult<Identity>;


                    Assert.False(response.IsSucceed);
                    Assert.NotNull(response.Error);
                    Assert.Null(response.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
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
                    var config = Storage.InitConfiguration();
                    var hashManager = new HashManager();
                    var accRep = new AccountRepository(context, config, hashManager);
                    var log = new LogRepository(config);
                    var tokenManager = new TokenManager(config);
                    var sup = new Supervisor(log, tokenManager);


                    var api = new AuthenticationController(config, accRep, sup, tokenManager);

                    var response =
                    (
                       await api.ValidateAsync(token) as JsonResult
                    ).Value as ExecutionResult<Identity>;


                    Assert.False(response.IsSucceed);
                    Assert.NotNull(response.Error);
                    Assert.Null(response.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }


        private void Compare(Account expected, Account actual)
        {
            Assert.Equal(expected.Login, actual.Login);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Password, actual.Password);
            Assert.Null(expected.Password);
            Assert.Equal(expected.Role, actual.Role);
            Assert.Equal(expected.Version, actual.Version);
        }
    }
}
