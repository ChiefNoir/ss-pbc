using BusinessService.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BusinessService.DataLayer
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<News> News { get; set; }

        public DataContext(DbContextOptions options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            MigrateDatabase(this.Database.GetDbConnection());
        }

        private void MigrateDatabase(IDbConnection connection)
        {
           
        }
    }
}
