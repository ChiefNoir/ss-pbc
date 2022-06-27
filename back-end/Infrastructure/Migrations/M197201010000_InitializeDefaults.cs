using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    [Migration(197201010000, "Initialize default values")]
    public class M197201010000_InitializeDefaults : Migration
    {
        internal static Guid categoryId = new("A4517F51-9C85-4850-BE3F-F196742925D2");
        internal static string categoryDisplayName = "Everything";
        internal static string categoryCode = "all";

        internal static Guid introductionId = new("4FFC4CC3-C51A-4619-901A-D0BAA9D702BC");
        internal static string introductionTitle = "Welcome";
        internal static string introductionContent = "The service is on-line. Congratulations.";

        private const string _categoryTable = "category";
        private const string _introductionTable = "introduction";

        public override void Up()
        {
            Execute.Sql(
                $@"INSERT INTO {DataContext.SchemaData}.{_categoryTable}(id, code, display_name, is_everything)
                    VALUES ('{categoryId}', '{categoryCode}', '{categoryDisplayName}', TRUE);");

            Execute.Sql(
                $@"INSERT INTO {DataContext.SchemaData}.{_introductionTable}(id, title, content)
                    VALUES ('{introductionId}', '{introductionTitle}', '{introductionContent}');");
        }

        public override void Down()
        {
            Execute.Sql($@"DELETE FROM {DataContext.SchemaData}.{_categoryTable} WHERE id = '{categoryId}';"); ;

            Execute.Sql($@"DELETE FROM {DataContext.SchemaData}.{_introductionTable} WHERE id = '{introductionId}';");
        }
    }
}
