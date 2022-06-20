using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GeneralTests")]
namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class DataContext : DbContext
    {
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<CategoryWithTotalProjects> CategoriesWithTotalProjects { get; set; }
        internal DbSet<ExternalUrl> ExternalUrls { get; set; }

        internal DbSet<Introduction> Introductions { get; set; }
        internal DbSet<IntroductionToExternalUrl> IntroductionExternalUrls { get; set; }

        internal DbSet<Project> Projects { get; set; }
        internal DbSet<ProjectToExternalUrl> ProjectExternalUrls { get; set; }

        public Migrator Migrator { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            Migrator = new Migrator(Database.GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectToExternalUrl>().HasKey(sc => new { sc.ProjectId, sc.ExternalUrlId });
            modelBuilder.Entity<IntroductionToExternalUrl>().HasKey(sc => new { sc.IntroductionId, sc.ExternalUrlId });
        }
    }
}
