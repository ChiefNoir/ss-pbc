using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicPingController_Tests
    {
        
        [Fact]
        internal void Ping_Test()
        {
            using (var context = Storage.CreateContext())
            {
                var api = Storage.CreatePublicController(context);

                var response =
                (
                    api.Ping() as JsonResult
                ).Value as ExecutionResult<string>;

                GenericChecks.CheckSucceed(response);

                Assert.Equal("pong", response.Data);
            }
        }

    }
}
