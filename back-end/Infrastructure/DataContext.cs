using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>Entity framework data context </summary>
    public class DataContext : DbContext
    {
        private static bool _isMigrationsDone;

        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<CategoryWithTotalProjects> CategoriesWithTotalProjects { get; set; }
        internal DbSet<ExternalUrl> ExternalUrls { get; set; }
        internal DbSet<GalleryImage> GalleryImages { get; set; }
        internal DbSet<Introduction> Introductions { get; set; }
        internal DbSet<IntroductionExternalUrl> IntroductionExternalUrls { get; set; }
        internal DbSet<Project> Projects { get; set; }
        internal DbSet<Log> Log { get; set; }
        internal DbSet<ProjectExternalUrl> ProjectExternalUrls { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            if (!_isMigrationsDone)
            {
                MigrateDatabase(Database.GetDbConnection());
                _isMigrationsDone = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectExternalUrl>().HasKey(sc => new { sc.ProjectId, sc.ExternalUrlId });
            modelBuilder.Entity<IntroductionExternalUrl>().HasKey(sc => new { sc.IntroductionId, sc.ExternalUrlId });
        }

        private static void MigrateDatabase(IDbConnection connection)
        {
            try
            {
                var evolve = new Evolve.Evolve(connection)
                {
                    EmbeddedResourceAssemblies = new[] { Assembly.GetExecutingAssembly() },
                    IsEraseDisabled = true,
                    EnableClusterMode = true
                };

                evolve.Migrate();
            }
            catch (Exception)
            {
                //TODO: log error, Database migration failed.
                throw;
            }
        }
    }
}