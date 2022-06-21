using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Security;
using SSPBC.Controllers;

namespace GeneralTests
{
    internal static class Initializer
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

            return context;
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
