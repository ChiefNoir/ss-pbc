using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Infrastructure.Repository;
using Security;
using Moq;
using Microsoft.Extensions.Logging;
using API.Controllers.Public;
using API.Controllers.Gateway;
using API.Controllers.Private;

namespace GeneralTests.SharedUtils
{
    public static class Storage
    {
        private const string connectionName = "Default";

        public static DataContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(CreateConfiguration().GetConnectionString(connectionName));

            var options = builder.Options;
            var context = new DataContext(options, null);

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

            using (var connection = new Npgsql.NpgsqlConnection(CreateConfiguration().GetConnectionString(connectionName)))
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



        internal static PublicController CreatePublicController(DataContext context)
        {
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(logger.Object);

            var catRep = new CategoryRepository(context);
            var intrRep = new IntroductionRepository(context);
            var prjRep = new ProjectRepository(context, catRep);

            return new PublicController (sup, catRep, intrRep, prjRep);
        }

        internal static GatewayController CreateGatewayController(DataContext context)
        {
            var config = CreateConfiguration();
            var tokenManager = new TokenManager(config);
            var hashManager = new HashManager(config);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(logger.Object);

            var accRep = new AccountRepository(context, config, hashManager);

            return new GatewayController
            (
                config, accRep, sup, tokenManager
            );
        }

        internal static PrivateController CreatePrivateController(DataContext context)
        {
            var config = CreateConfiguration();
            var tokenManager = new TokenManager(config);
            var hashManager = new HashManager(config);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(logger.Object);


            var accRep = new AccountRepository(context, config, hashManager);
            var catRep = new CategoryRepository(context);
            var intrRep = new IntroductionRepository(context);
            var prjRep = new ProjectRepository(context, catRep);
            var fileRep = new FileRepository(config);


            return new PrivateController
            (
                accRep, catRep, config, fileRep, intrRep, prjRep, sup, tokenManager
            );
        }
    }
}
