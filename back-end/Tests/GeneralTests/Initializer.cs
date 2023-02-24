using Abstractions.Cache;
using Infrastructure;
using Infrastructure.Cache;
using Infrastructure.Repositories;
using Infrastructure.RepositoriesPrivate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Security;
using SSPBC.Admin.Controllers;
using SSPBC.Controllers;
using StackExchange.Redis;

namespace GeneralTests
{
    internal static class Initializer
    {
        const string KeyDatabase = "PostgreSQL";
        const string KeyCache = "Redis";
        const string CachePrefix = "Test";

        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        }

        public static Tuple<DataContext, IDataCache> CreateDataContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(CreateConfiguration().GetConnectionString(KeyDatabase));
            var context = new DataContext(builder.Options);

            var options = ConfigurationOptions.Parse(CreateConfiguration().GetConnectionString(KeyCache));
            var multiplexer = ConnectionMultiplexer.Connect(options);

            var cache = new DataCache(multiplexer, CachePrefix);

            Task.Run(cache.FlushAsync);

            return new Tuple<DataContext, IDataCache>(context, cache);
        }

        public static PublicController CreatePublicController(DataContext context, IDataCache cache)
        {
            var sup = new Supervisor();

            var catRep = new CategoryRepository(context, cache);
            var intrRep = new IntroductionRepository(context, cache);
            var prjRep = new ProjectRepository(context, catRep, cache);

            return new PublicController(sup, catRep, intrRep, prjRep);
        }

        public static GatewayController CreateGatewayController(DataContext context)
        {
            var conf = CreateConfiguration();
            var hashManager = new HashManager(conf);
            var accRep = new AccountRepository(context, conf, hashManager);
            var sessionRep = new SessionRepository(context);
            var sup = new Supervisor();
            var tokenManager = new TokenManager(conf);

            return new GatewayController(conf, accRep, tokenManager, sessionRep, sup);
        }

        public static PrivateController CreatePrivateController(DataContext context, IDataCache cache)
        {
            var conf = CreateConfiguration();

            var hashManager = new HashManager(conf);
            var accRep = new AccountRepository(context, conf, hashManager);
            var fileRep = new FileRepository(conf);
            var intrRep = new PrivateIntroductionRepository(context, cache);
            var catRep = new PrivateCategoryRepository(context, cache);
            var prjRep = new PrivateProjectRepository(context, cache);
            var sessRep = new SessionRepository(context);
            var sup = new Supervisor();

            return new PrivateController(accRep, catRep, conf, fileRep, intrRep, prjRep, sessRep, sup);
        }
    }
}
