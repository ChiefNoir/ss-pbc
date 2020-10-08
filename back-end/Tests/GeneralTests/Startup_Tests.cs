using API;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
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

                    webHost.UseConfiguration(Storage.CreateConfiguration());
                    webHost.UseStartup<Startup>();
                });

            // Build and start the IHost
            using var host = await hostBuilder.StartAsync();
            try
            {
                using var client = host.GetTestClient();
                using var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Status Code 200-299
                Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await host.StopAsync();
                host.Dispose();
            }
        }
    }

}
