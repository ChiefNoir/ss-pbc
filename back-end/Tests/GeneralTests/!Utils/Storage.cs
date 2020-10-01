using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTests.Utils
{
    public static class Storage
    {
        public static DataContext CreateContextInMemory<T>(T initialItem) where T : class
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase("temp");

            var options = builder.Options;
            var context = new DataContext(options);
            context.Database.EnsureCreated();

            if (initialItem != null)
            {
                context.Set<T>().Add(initialItem);
                context.SaveChanges();
            }

            return context;
        }

        public static DataContext CreateContext<T>(T initialItem) where T : class
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(InitConfiguration().GetConnectionString("Test"));

            var options = builder.Options;
            var context = new DataContext(options);

            if (initialItem != null)
            {
                context.Set<T>().Add(initialItem);
                context.SaveChanges();
            }

            return context;
        }

        public static DataContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(InitConfiguration().GetConnectionString("Test"));

            var options = builder.Options;
            var context = new DataContext(options);

            return context;
        }







        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

    }
}
