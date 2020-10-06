using Abstractions.Supervision;
using API.Controllers.Public;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicPingController_Tests
    {
        [Fact]
        internal void Ping_Test()
        {
            var config = Storage.InitConfiguration();
            var log = new LogRepository(config);
            var tokenManager = new TokenManager(config);
            var sup = new Supervisor(log, tokenManager);
            var api = new PublicPingController(sup);

            var response = (api.Ping() as JsonResult).Value as ExecutionResult<string>;

            Assert.True(response.IsSucceed);
            Assert.Null(response.Error);
            Assert.NotNull(response.Data);
            Assert.Equal("pong", response.Data);
        }

    }
}
