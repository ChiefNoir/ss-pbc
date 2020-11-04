using Abstractions.Model.System;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests.API.Controllers.Private
{
    public class PrivateInformationalController_Test
    {
        [Fact]
        internal async Task GetInformationAsync()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = Storage.CreateGatewayController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    GenericChecks.CheckSucceed(identity);

                    var httpContext = new DefaultHttpContext();
                    httpContext.Request.Headers["Authorization"] = identity.Data.Token;
                    var controllerContext = new ControllerContext()
                    {
                        HttpContext = httpContext,
                    };

                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = controllerContext;
                    

                    var response =
                    (
                        api.GetInformationAsync() as JsonResult
                    ).Value as ExecutionResult<Information>;

                    GenericChecks.CheckSucceed(response);

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
                    context.FlushData();
                }
            }
        }


        [Theory]
        [InlineData(null)]
        [InlineData("bad-token")]
        [InlineData("")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2EiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTYwMTk5Njk3MSwiZXhwIjoxNjAxOTk4NzcxLCJpc3MiOiJJc3N1ZXJOYW1lIiwiYXVkIjoiQXVkaWVuY2UtMSJ9.DCbppW8SqvL1QJS2BIO2qlplZv-UHqI2_NP_Za0KDzA")]
        internal void GetInformation_BadToken(string token)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = ControllerContextCreator.CreateInvalid(token);

                    var response =
                    (
                        api.GetInformationAsync() as JsonResult
                    ).Value as ExecutionResult<Information>;

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

    }
}
