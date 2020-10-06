using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Controllers.Public;
using API.Model;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }
}
