using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public sealed class Migrator
    {
        private readonly string _connectionString = string.Empty;

        public Migrator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void MigrateUp()
        {
            using (var scope = CreateServices(_connectionString).CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.ListMigrations();

                // Execute the migrations
                runner.MigrateUp();
            }
        }

        public void MigrateDown(long version)
        {
            using (var scope = CreateServices(_connectionString).CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.ListMigrations();

                // Execute the migrations
                runner.MigrateDown(version);
            }
        }


        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Migrator).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
    }
}
