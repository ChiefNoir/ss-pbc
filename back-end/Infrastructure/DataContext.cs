using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GeneralTests")]
namespace Infrastructure
{
    public class DataContext : DbContext
    {
        public const string SchemaData = "data";
        public const string SchemaHistory = "history";

        internal DbSet<Account> Accounts => Set<Account>();
        internal DbSet<Category> Categories => Set<Category>();
        internal DbSet<CategoryWithTotalProjects> CategoriesWithTotalProjects => Set<CategoryWithTotalProjects>();
        internal DbSet<ExternalUrl> ExternalUrls => Set<ExternalUrl>();

        internal DbSet<Introduction> Introductions => Set<Introduction>();
        internal DbSet<IntroductionToExternalUrl> IntroductionExternalUrls => Set<IntroductionToExternalUrl>();

        internal DbSet<Project> Projects => Set<Project>();
        internal DbSet<ProjectToExternalUrl> ProjectExternalUrls => Set<ProjectToExternalUrl>();

        public Migrator Migrator { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            Migrator = new Migrator(Database.GetConnectionString());

            //TODO: fix it
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaData);

            modelBuilder.Entity<ProjectToExternalUrl>().HasKey(sc => new { sc.ProjectId, sc.ExternalUrlId });
            modelBuilder.Entity<IntroductionToExternalUrl>().HasKey(sc => new { sc.IntroductionId, sc.ExternalUrlId });
        }
    }
}
