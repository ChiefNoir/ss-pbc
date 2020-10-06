using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Controllers.Private;
using API.Model;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using System;
using Xunit;

namespace GeneralTests.API.Controllers.Private
{
    public class PrivateInformationalController_Test
    {
        [Fact]
        internal async void GetInformation()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var logRep = new LogRepository(confing);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateInformationalController(sup, tokenManager);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);

                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var response =
                    (
                        api.GetInformation(identity.Data.Token) as JsonResult
                    ).Value as ExecutionResult<Information>;

                    Assert.NotNull(response.Data);
                    Assert.Null(response.Error);
                    Assert.True(response.IsSucceed);

                    Assert.Equal(identity.Data.Account.Role, response.Data.Role);
                    Assert.Equal(identity.Data.Account.Login, response.Data.Login);
                    Assert.NotNull(response.Data.ApiVersion);
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
        [InlineData(null)]
        [InlineData("bad-token")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2EiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTYwMTk5Njk3MSwiZXhwIjoxNjAxOTk4NzcxLCJpc3MiOiJJc3N1ZXJOYW1lIiwiYXVkIjoiQXVkaWVuY2UtMSJ9.DCbppW8SqvL1QJS2BIO2qlplZv-UHqI2_NP_Za0KDzA")]
        internal void GetInformation_BadToken(string token)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var logRep = new LogRepository(confing);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateInformationalController(sup, tokenManager);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);


                    var response =
                    (
                        api.GetInformation(token) as JsonResult
                    ).Value as ExecutionResult<Information>;

                    Assert.Null(response.Data);
                    Assert.NotNull(response.Error);
                    Assert.False(response.IsSucceed);
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
