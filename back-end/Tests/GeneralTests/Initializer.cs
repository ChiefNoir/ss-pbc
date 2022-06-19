using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Security;
using Security.Models;
using SSPBC.Controllers;
using SSPBC.Models;

namespace GeneralTests
{
    internal class Initializer
    {
        private const string connectionName = "Default";

        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        }

        public static DataContext CreateDataContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(CreateConfiguration().GetConnectionString(connectionName));
            var context = new DataContext(builder.Options);
            context.Migrator.MigrateDown(0);

            return context;
        }

        public static async Task<ControllerContext> CreateControllerContext_Valid(DataContext context)
        {
            var apiAuth = Initializer.CreateGatewayController(context);
            var identity =
            (
                (JsonResult) await apiAuth.LoginAsync
                (
                    new Credentials { Login = "sa", Password = "sa" }
                )
            ).Value as ExecutionResult<Identity>;

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = identity!.Data!.Token;

            return new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }

        public static ControllerContext CreateControllerContext_Invalid(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;

            return new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }

        public static PublicController CreatePublicController(DataContext context)
        {
            var sup = new Supervisor();
            var catRep = new CategoryRepository(context);
            var intrRep = new IntroductionRepository(context);
            var prjRep = new ProjectRepository(context, catRep);

            return new PublicController(sup, catRep, intrRep, prjRep);
        }

        public static GatewayController CreateGatewayController(DataContext context)
        {
            var conf = CreateConfiguration();
            var hashManager = new HashManager(conf);
            var accRep = new AccountRepository(context, conf, hashManager);
            var sup = new Supervisor();
            var tokenManager = new TokenManager(conf);

            return new GatewayController(conf, accRep, sup, tokenManager);
        }

        public static PrivateController CreatePrivateController(DataContext context)
        {
            var conf = CreateConfiguration();
            var hashManager = new HashManager(conf);
            var accRep = new AccountRepository(context, conf, hashManager);
            var fileRep = new FileRepository(conf);
            var intrRep = new IntroductionRepository(context);
            var catRep = new CategoryRepository(context);
            var prjRep = new ProjectRepository(context, catRep);
            var sup = new Supervisor();
            var tokenManager = new TokenManager(conf);

            return new PrivateController(accRep, catRep, conf, fileRep, intrRep, prjRep, sup, tokenManager);
        }
    }
}
