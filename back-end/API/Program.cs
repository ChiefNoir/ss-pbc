using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace API
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                           webBuilder.UseUrls();
                           webBuilder.UseKestrel();
                           webBuilder.ConfigureKestrel(serverOptions =>
                           {
                               serverOptions.ConfigureEndpointDefaults(listenOptions =>
                               {
                                   listenOptions.UseHttps();
                               });
                           });
                       });
        }
    }
}