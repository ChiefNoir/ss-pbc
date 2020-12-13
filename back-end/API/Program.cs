using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

[assembly: CLSCompliant(false)]
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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                           webBuilder.UseUrls();
                           webBuilder.UseConfiguration(configuration);
                           webBuilder.UseKestrel();
                       });
        }
    }
}