using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace GeneralTests.SharedUtils
{
    public static class Storage
    {
        public static DataContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(CreateConfiguration().GetConnectionString("Test"));

            var options = builder.Options;
            var context = new DataContext(options);

            return context;
        }

        public static void RunSql(string[] sql)
        {
            if (sql == null || !sql.Any())
                return;

            foreach (var item in sql)
            {
                RunSql(item);
            }
        }

        public static void RunSql(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return;

            using (var connection = new Npgsql.NpgsqlConnection(CreateConfiguration().GetConnectionString("Test")))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = sql;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public static IConfiguration CreateConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

    }
}
