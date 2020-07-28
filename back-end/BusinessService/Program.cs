using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BusinessService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
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