using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(197001010000, "Initialize database")]
    public class M19700101_Init : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"CREATE TABLE introduction 
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
                @"CREATE TABLE category 
                (
	                id uuid NOT NULL UNIQUE,
	                code varchar(128) NOT NULL UNIQUE,
	                display_name VARCHAR(128) NOT NULl,
	                is_everything BOOLEAN NOT NULL DEFAULT FALSE,
	                version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
                );");
            Execute.Sql(
                @"ALTER TABLE category ADD CONSTRAINT CK_category_code__only_latin_and_numbers
						CHECK (code ~* '^[a-z0-9_.-]*$');");

            Execute.Sql(
                @"ALTER TABLE category ADD CONSTRAINT CK_category_code__only_lowercase
						CHECK (code = LOWER(code));");



            Execute.Sql(
                @"CREATE TABLE project
				(
					id uuid NOT NULL UNIQUE,
					code VARCHAR(128) NOT NULL UNIQUE,
					display_name VARCHAR(128) NOT NULL,
					release_date DATE, 
					poster_url VARCHAR(2000) NULL,
					poster_description VARCHAR(512) NULL,
					category_id uuid REFERENCES CATEGORY (id),
					description_short TEXT,
					description TEXT,
					version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
				);");
            Execute.Sql(
                @"ALTER TABLE project ADD CONSTRAINT CK_project_code__only_latin_and_numbers
						CHECK (code ~* '^[a-z0-9_.-]*$');");

            Execute.Sql(
                @"ALTER TABLE project ADD CONSTRAINT CK_project_code__only_lowercase
						CHECK (code = LOWER(Code));");

            Execute.Sql(
                @"CREATE TABLE external_url
				(
					id uuid NOT NULL UNIQUE,
					url VARCHAR(2000) NOT NULL,
					display_name VARCHAR(128) NOT NULL,
					version INT NOT NULL DEFAULT 0,

					PRIMARY KEY(id)
				);");

            Execute.Sql(
                @"CREATE TABLE account
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
                @"CREATE TABLE project_to_external_url
				(
					project_id uuid REFERENCES project (id) ON DELETE CASCADE,
					external_url_id uuid REFERENCES external_url (id) ON DELETE CASCADE,

					PRIMARY KEY(project_id, external_url_id)
				);");

            Execute.Sql(
                @"CREATE TABLE introduction_to_external_url
				(
					introduction_id uuid REFERENCES introduction (id) ON DELETE CASCADE,
					external_url_id uuid REFERENCES external_url (id) ON DELETE CASCADE,

					PRIMARY KEY(introduction_id, external_url_id)
				);");

            Execute.Sql(
                @"CREATE VIEW categories_with_projects_total_v AS
				(
					SELECT 
						c.*, COALESCE(sm.total, 0) AS total_projects
					FROM 
						category c
						LEFT JOIN
						(
							SELECT
								COALESCE(category_id, (SELECT id FROM category WHERE is_everything = TRUE LIMIT 1) ) AS id, 
								COUNT(1) AS total
							FROM 
								project 
							GROUP BY
								ROLLUP(category_id)
						) sm 
					ON 
						c.id = sm.id
					ORDER BY 
						total DESC
				);");
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE IF EXISTS project_to_external_url CASCADE;");
            Execute.Sql("DROP TABLE IF EXISTS introduction_to_external_url CASCADE;");

            Execute.Sql("DROP TABLE IF EXISTS introduction CASCADE;");
            Execute.Sql("DROP TABLE IF EXISTS category CASCADE;");
            Execute.Sql("DROP TABLE IF EXISTS project CASCADE;");
            Execute.Sql("DROP TABLE IF EXISTS external_url CASCADE;");
            Execute.Sql("DROP TABLE IF EXISTS account CASCADE;");

            Execute.Sql("DROP VIEW IF EXISTS categories_with_projects_total_v CASCADE;");
        }
    }
}