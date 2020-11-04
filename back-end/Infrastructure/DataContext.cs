using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GeneralTests")]
namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    /// <summary>Entity framework data context </summary>
    public class DataContext : DbContext
    {
        private static volatile bool _isMigrationsDone;
        private static readonly object _migrationLock = new object();
        private readonly ILogger<DataContext> _logger;


        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<CategoryWithTotalProjects> CategoriesWithTotalProjects { get; set; }
        internal DbSet<ExternalUrl> ExternalUrls { get; set; }
        internal DbSet<GalleryImage> GalleryImages { get; set; }
        internal DbSet<Introduction> Introductions { get; set; }
        internal DbSet<IntroductionExternalUrl> IntroductionExternalUrls { get; set; }
        internal DbSet<Project> Projects { get; set; }
        internal DbSet<ProjectExternalUrl> ProjectExternalUrls { get; set; }

        public DataContext(DbContextOptions options, ILogger<DataContext> logger) : base(options)
        {
            _logger = logger;

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
                return;

            if (_isMigrationsDone) 
                return;

            lock (_migrationLock)
            {
                if (_isMigrationsDone)
                    return;

                MigrateDatabase(Database.GetDbConnection());
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectExternalUrl>().HasKey(sc => new { sc.ProjectId, sc.ExternalUrlId });
            modelBuilder.Entity<IntroductionExternalUrl>().HasKey(sc => new { sc.IntroductionId, sc.ExternalUrlId });
        }

        /// <summary> [WARNING] Remove all tables from database</summary>
        internal void FlushData()
        {
            using (var connection = Database.GetDbConnection())
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText =
                        "DO $$ DECLARE tabname RECORD; " +
                        "BEGIN " +
                            "FOR tabname IN(SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) " +
                            "LOOP " +
                                "EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(tabname.tablename) || ' CASCADE'; " +
                            "END LOOP; " +
                        "END $$; ";

                    connection.Open();
                    command.ExecuteNonQuery();

                    MigrationUndone();
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        private void MigrateDatabase(IDbConnection connection)
        {
            try
            {
                var evolve = new Evolve.Evolve(connection)
                {
                    EmbeddedResourceAssemblies = new[] { Assembly.GetExecutingAssembly() },
                    IsEraseDisabled = true,
                    EnableClusterMode = false
                };

                evolve.Migrate();

                MigrationDone();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "Database migration failed");
                throw;
            }
        }

        private static void MigrationUndone()
        {
            _isMigrationsDone = false;
        }

        private static void MigrationDone()
        {
            _isMigrationsDone = true;
        }
    }
}
