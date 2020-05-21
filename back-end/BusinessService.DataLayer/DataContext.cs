using BusinessService.DataLayer.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ServerSetting> ServerSettings { get; set; }

        public DataContext(DbContextOptions options) : base(options) 
        {
            MigrateDatabase(this.Database.GetDbConnection());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
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
