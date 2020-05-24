using BusinessService.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Reflection;

namespace BusinessService.DataLayer
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ServerSetting> ServerSettings { get; set; }

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
