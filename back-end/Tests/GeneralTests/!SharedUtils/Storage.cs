using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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




        internal static PrivateCategoryController CreatePrivateCategoryController(DataContext context)
        {
            var confing = Storage.CreateConfiguration();
            var categoryRep = new CategoryRepository(context);
            var tokenManager = new TokenManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new PrivateCategoryController(categoryRep, sup);
        }

        internal static PublicCategoryController CreatePublicCategoryController(DataContext context)
        {
            var confing = Storage.CreateConfiguration();
            var catRep = new CategoryRepository(context);
            var tokenManager = new TokenManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);
            return new PublicCategoryController(catRep, sup);
        }

        internal static AuthenticationController CreateAuthenticationController(DataContext context)
        {
            var confing = Storage.CreateConfiguration();
            var hashManager = new HashManager(confing);
            var accountRep = new AccountRepository(context, confing, hashManager);
            var tokenManager = new TokenManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new AuthenticationController(confing, accountRep, sup, tokenManager);
        }

        internal static PublicProjectController CreatePublicProjectController(DataContext context)
        {
            var categoryRep = new CategoryRepository(context);
            var projectRep = new ProjectRepository(context, categoryRep);
            var tokenManager = new TokenManager(Storage.CreateConfiguration());
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new PublicProjectController(projectRep, sup);
        }

        internal static PrivateProjectController CreatePrivateProjectController(DataContext context)
        {
            var config = Storage.CreateConfiguration();
            var fileRep = new FileRepository(config);
            var categoryRep = new CategoryRepository(context);
            var projectRep = new ProjectRepository(context, categoryRep);
            var tokenManager = new TokenManager(config);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new PrivateProjectController(config, fileRep, projectRep, sup);
        }

        internal static PrivateAccountController CreatePrivateAccountController(DataContext context)
        {
            var confing = CreateConfiguration();
            var tokenManager = new TokenManager(confing);
            var hasManager = new HashManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);
            var accRep = new AccountRepository(context, confing, hasManager);

            return new PrivateAccountController(accRep, sup);
        }

        internal static PrivateInformationalController CreatePrivateInformationalController(DataContext context)
        {
            var confing = CreateConfiguration();
            var tokenManager = new TokenManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new PrivateInformationalController(sup, tokenManager);
        }

        internal static PrivateIntroductionController CreatePrivateIntroductionController(DataContext context)
        {
            var confing = CreateConfiguration();
            var fileRep = new FileRepository(confing);
            var introductionRep = new IntroductionRepository(context);
            var tokenManager = new TokenManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new PrivateIntroductionController(fileRep, confing, introductionRep, sup);
        }

        internal static PublicIntroductionController CreatePublicIntroductionController(DataContext context)
        {
            var confing = CreateConfiguration();
            var introductionRep = new IntroductionRepository(context);
            var tokenManager = new TokenManager(confing);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);

            return new PublicIntroductionController(introductionRep, sup);
        }

        internal static PublicPingController CreatePublicPingController()
        {
            var config = Storage.CreateConfiguration();
            var tokenManager = new TokenManager(config);
            var logger = new Mock<ILogger<Supervisor>>();
            var sup = new Supervisor(tokenManager, logger.Object);
            return new PublicPingController(sup);
        }

    }
}
