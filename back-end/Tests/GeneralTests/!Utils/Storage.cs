using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace GeneralTests.Utils
{
    public static class Storage
    {
        public static DataContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(InitConfiguration().GetConnectionString("Test"));

            var options = builder.Options;
            var context = new DataContext(options);

            return context;
        }

        public static void RunSql(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return;

            using (var connection = new Npgsql.NpgsqlConnection(InitConfiguration().GetConnectionString("Test")))
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


        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

    }
}
