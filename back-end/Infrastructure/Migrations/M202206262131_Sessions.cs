using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    [Migration(202206262131, "Sessions management tables")]
    public sealed class M202206262131_Sessions : Migration
    {
        private static string _sessionTable = "session";
        private const string _accountTable = "account";

        public override void Up()
        {
            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_sessionTable} 
                (
	                id uuid NOT NULL UNIQUE,
                    account_id uuid NOT NULL 
                            REFERENCES {DataContext.SchemaData}.{_accountTable} (id) ON DELETE CASCADE,
	                token TEXT NOT NULL,
                    fingerprint VARCHAR(2000) NOT NULL,

					PRIMARY KEY(id)
                );");

            Execute.Sql($@"CREATE INDEX session__token__idx ON {DataContext.SchemaData}.{_sessionTable}(token ASC NULLS LAST);");
        }

        public override void Down()
        {
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_sessionTable} CASCADE;");
        }
    }
}
