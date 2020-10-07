using Abstractions.Supervision;
using API.Controllers.Public;
using GeneralTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Security;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicPingController_Tests
    {
        private static PublicPingController CreatePublicPingController()
        {
            var config = Storage.CreateConfiguration();
            var tokenManager = new TokenManager(config);
            var sup = new Supervisor(tokenManager);
            return new PublicPingController(sup);
        }

        [Fact]
        internal void Ping_Test()
        {
            var api = CreatePublicPingController();

            var response =
            (
                api.Ping() as JsonResult
            ).Value as ExecutionResult<string>;

            GenericChecks.CheckSucceed(response);

            Assert.Equal("pong", response.Data);
        }

    }
}
