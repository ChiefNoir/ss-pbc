using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(197101010000, "Initialize default values")]
    public class M197101010000_Default : Migration
    {
        internal static Guid categoryId = new("A4517F51-9C85-4850-BE3F-F196742925D2");
        internal static Guid introductionId = new("00000000-0000-0000-0000-000000000000");

        public override void Up()
        {
            Execute.Sql(
                $@"INSERT INTO category(id, code, display_name, is_everything)
                    VALUES ('{categoryId}', 'all', 'Everything', TRUE);");

            Execute.Sql(
                $@"INSERT INTO introduction(id, title, content)
                    VALUES ('{introductionId}', 'Hello', 'The service is on-line. Congratulations.');");
        }

        public override void Down()
        {
            Execute.Sql($@"DELETE FROM category WHERE id = '{categoryId}';"); ;

            Execute.Sql($@"DELETE FROM introduction WHERE id = '{introductionId}';");
        }
    }
}
