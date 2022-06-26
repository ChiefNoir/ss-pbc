using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Migrations
{
	[ExcludeFromCodeCoverage]
	[Migration(197101010000, "Initialize core tables")]
	public sealed class M197101010000_BaseTables : Migration
    {
		private const string _introductionTable = "introduction";
		private const string _categoryTable = "category";
		private const string _projectTable = "project";
		private const string _externalUrlTable = "external_url";
		private const string _accountTable = "account";

		public override void Up()
        {
            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_introductionTable} 
                (
	                id uuid NOT NULL UNIQUE,
	                title TEXT,
	                poster_url VARCHAR(2000) NULL,
	                poster_description VARCHAR(512) NULL,
	                content TEXT,
	                version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
                );");

            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_categoryTable} 
                (
	                id uuid NOT NULL UNIQUE,
	                code varchar(128) NOT NULL UNIQUE,
	                display_name VARCHAR(128) NOT NULl,
	                is_everything BOOLEAN NOT NULL DEFAULT FALSE,
	                version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
                );");
            Execute.Sql(
                $@"ALTER TABLE {DataContext.SchemaData}.{_categoryTable} ADD CONSTRAINT {_categoryTable}__code__only_latin_and_numbers__ck
						CHECK (code ~* '^[a-z0-9_.-]*$');");
            Execute.Sql(
                $@"ALTER TABLE {DataContext.SchemaData}.{_categoryTable} ADD CONSTRAINT {_categoryTable}__code__only_lowercase__ck
						CHECK (code = LOWER(code));");

            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_projectTable}
				(
					id uuid NOT NULL UNIQUE,
					code VARCHAR(128) NOT NULL UNIQUE,
					name VARCHAR(128) NOT NULL,
					release_date DATE, 
					poster_url VARCHAR(2000) NULL,
					poster_description VARCHAR(512) NULL,
					category_id uuid REFERENCES {DataContext.SchemaData}.{_categoryTable} (id),
					description_short TEXT,
					description TEXT,
					version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
				);");
            Execute.Sql(
                $@"ALTER TABLE {DataContext.SchemaData}.{_projectTable} ADD CONSTRAINT {_projectTable}__code__only_latin_and_numbers__ck
						CHECK (code ~* '^[a-z0-9_.-]*$');");
            Execute.Sql(
                $@"ALTER TABLE {DataContext.SchemaData}.{_projectTable} ADD CONSTRAINT {_projectTable}__code__only_lowercase__ck
						CHECK (code = LOWER(Code));");

            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_externalUrlTable}
				(
					id uuid NOT NULL UNIQUE,
					url VARCHAR(2000) NOT NULL,
					display_name VARCHAR(128) NOT NULL,
					version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
				);");

            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_accountTable}
				(
					id uuid NOT NULL UNIQUE,
					login VARCHAR(256) NOT NULL UNIQUE,
					password VARCHAR(256) NOT NULL,	
					salt VARCHAR(256) NOT NULL,
					role VARCHAR(128) NOT NULL,
					version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
				);");


            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_projectTable}_to_{_externalUrlTable}
				(
					project_id uuid REFERENCES {DataContext.SchemaData}.{_projectTable} (id) ON DELETE CASCADE,
					external_url_id uuid REFERENCES {DataContext.SchemaData}.{_externalUrlTable} (id) ON DELETE CASCADE,

					PRIMARY KEY(project_id, external_url_id)
				);");

            Execute.Sql(
                $@"CREATE TABLE {DataContext.SchemaData}.{_introductionTable}_to_{_externalUrlTable}
				(
					introduction_id uuid REFERENCES {DataContext.SchemaData}.{_introductionTable} (id) ON DELETE CASCADE,
					external_url_id uuid REFERENCES {DataContext.SchemaData}.{_externalUrlTable} (id) ON DELETE CASCADE,

					PRIMARY KEY(introduction_id, external_url_id)
				);");

            Execute.Sql(
                $@"CREATE VIEW {DataContext.SchemaData}.categories_with_projects_total_v AS
				(
					SELECT 
						c.*, COALESCE(sm.total, 0) AS total_projects
					FROM 
						{DataContext.SchemaData}.{_categoryTable} c
						LEFT JOIN
						(
							SELECT
								COALESCE(category_id, (SELECT id FROM {DataContext.SchemaData}.{_categoryTable} WHERE is_everything = TRUE LIMIT 1) ) AS id, 
								COUNT(1) AS total
							FROM 
								{DataContext.SchemaData}.{_projectTable} 
							GROUP BY
								ROLLUP(category_id)
						) sm 
					ON 
						c.id = sm.id
					ORDER BY 
						total DESC
				);");

            Execute.Sql($@"CREATE INDEX project__releasedate__idx ON {DataContext.SchemaData}.{_projectTable}(release_date ASC NULLS LAST);");
        }

        public override void Down()
        {
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_projectTable}_to_{_externalUrlTable} CASCADE;");
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_introductionTable}_to_{_externalUrlTable} CASCADE;");

            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_introductionTable} CASCADE;");
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_categoryTable} CASCADE;");
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_projectTable} CASCADE;");
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_externalUrlTable} CASCADE;");
            Execute.Sql(@$"DROP TABLE IF EXISTS {DataContext.SchemaData}.{_accountTable} CASCADE;");

            Execute.Sql(@$"DROP VIEW IF EXISTS {DataContext.SchemaData}.categories_with_projects_total__v CASCADE;");
        }
    }
}