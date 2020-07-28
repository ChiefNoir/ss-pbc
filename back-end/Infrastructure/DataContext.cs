using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>Entity framework data context </summary>
    public class DataContext : DbContext
    {
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<CategoryWithTotalProjects> CategoriesWithTotalProjects { get; set; }
        internal DbSet<ExternalUrl> ExternalUrls { get; set; }
        internal DbSet<IntroductionExternalUrl> IntroductionExternalUrls { get; set; }
        internal DbSet<Introduction> Introductions { get; set; }
        internal DbSet<ProjectExternalUrl> ProjectExternalUrls { get; set; }
        internal DbSet<Project> Projects { get; set; }

        /// <summary> Database has any accounts?</summary>
        internal bool HasAccounts { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            MigrateDatabase(Database.GetDbConnection()); //TODO: not good
            HasAccounts = Accounts.Any(); //TODO: not that good
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectExternalUrl>().HasKey(sc => new { sc.ProjectId, sc.ExternalUrlId });
            modelBuilder.Entity<IntroductionExternalUrl>().HasKey(sc => new { sc.IntroductionId, sc.ExternalUrlId });
        }

        private void MigrateDatabase(IDbConnection connection)
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
                //TODO: _logger.LogCritical("Database migration failed.", ex);
                throw;
            }
        }
    }
}