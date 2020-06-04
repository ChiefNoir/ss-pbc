using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Reflection;


namespace Infrastructure
{
    public class DataContext : DbContext
    {
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<News> News { get; set; }
        internal DbSet<Project> Projects { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            MigrateDatabase(Database.GetDbConnection());
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
