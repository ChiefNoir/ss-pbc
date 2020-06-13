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
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<News> News { get; set; }
        internal DbSet<Project> Projects { get; set; }
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<CategoryWithTotalProjects> CategoriesWithTotalProjects { get; set; }

        /// <summary> Database has any accounts?</summary>
        internal bool HasAccounts { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            MigrateDatabase(Database.GetDbConnection()); //TODO: not good
            HasAccounts = Accounts.Any(); //TODO: not that good
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
