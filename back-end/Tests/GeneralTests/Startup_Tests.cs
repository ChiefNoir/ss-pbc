using API;
using GeneralTests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests
{
    //Note: just to make sure
    public class Startup_Tests
    {
        [Theory]
        [InlineData("/api/v1/ping")]
        public async Task Get_EndpointsReturnSuccess(string url)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();

                    webHost.UseConfiguration(Storage.InitConfiguration());
                    webHost.UseStartup<Startup>();
                });

            // Build and start the IHost
            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }

}
