using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(197101010000, "Initialize default values")]
    public class M197101010000_Default : Migration
    {
        internal static Guid categoryId = new("A4517F51-9C85-4850-BE3F-F196742925D2");
        internal static string categoryDisplayName = "Everything";
        internal static string categoryCode = "all";

        internal static Guid introductionId = new("4FFC4CC3-C51A-4619-901A-D0BAA9D702BC");
        internal static string introductionTitle = "Hello";
        internal static string introductionContent = "The service is on-line. Congratulations.";

        public override void Up()
        {
            Execute.Sql(
                $@"INSERT INTO category(id, code, display_name, is_everything)
                    VALUES ('{categoryId}', '{categoryCode}', '{categoryDisplayName}', TRUE);");

            Execute.Sql(
                $@"INSERT INTO introduction(id, title, content)
                    VALUES ('{introductionId}', '{introductionTitle}', '{introductionContent}');");
        }

        public override void Down()
        {
            Execute.Sql($@"DELETE FROM category WHERE id = '{categoryId}';"); ;

            Execute.Sql($@"DELETE FROM introduction WHERE id = '{introductionId}';");
        }
    }
}
