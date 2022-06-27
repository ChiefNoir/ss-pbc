using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    [Migration(197001010000, "Initialize schema")]
    public sealed class M197001010000_InitializeSchema : Migration
    {
        public override void Up()
        {
            Create.Schema(DataContext.SchemaData);
        }

        public override void Down()
        {
            Delete.Schema(DataContext.SchemaData);
        }
    }
}
