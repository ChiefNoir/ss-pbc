-- introduction
CREATE TABLE introduction 
(
	id SERIAL PRIMARY KEY,
	title TEXT,
	poster_url VARCHAR(2000),
	poster_description VARCHAR(512),
	content TEXT,
	version INT NOT NULL DEFAULT 0
);
-- --------------------------------------------------------------

-- category
CREATE TABLE category 
(
	id SERIAL PRIMARY KEY,
	code varchar(128) NOT NULL UNIQUE, -- every category have unique code to identify category with short friendly name for inner use
	display_name VARCHAR(128) NOT NULl,
	is_everything BOOLEAN NOT NULL DEFAULT FALSE,
	version INT NOT NULL DEFAULT 0
);
-- --------------------------------------------------------------

-- project
CREATE TABLE project
(
	id SERIAL PRIMARY KEY,
	code VARCHAR(128) NOT NULL UNIQUE,-- every project have unique code to identify project with short friendly name for inner use
	display_name VARCHAR(128) NOT NULL,
	release_date DATE, 
	poster_url VARCHAR(2000), -- URLs over 2,000 characters will not work in the most popular web browsers
	poster_description VARCHAR(512),
	category_id INT REFERENCES CATEGORY (id),
	description_short TEXT,
	description TEXT,
	version INT NOT NULL DEFAULT 0
);
-- --------------------------------------------------------------

-- gallery_image
CREATE TABLE gallery_image
(
	id serial PRIMARY KEY,

	extra_url VARCHAR(2000), -- URLs over 2,000 characters will not work in the most popular web browsers
	image_url VARCHAR(2000) NOT NULL, -- URLs over 2,000 characters will not work in the most popular web browsers

	project_id INT REFERENCES PROJECT (id) ON DELETE CASCADE,
	version INT NOT NULL DEFAULT 0
);
-- --------------------------------------------------------------

-- external_url
CREATE TABLE external_url
(
	id SERIAL PRIMARY KEY,
	url VARCHAR(2000) NOT NULL, -- URLs over 2,000 characters will not work in the most popular web browsers
	display_name VARCHAR(128) NOT NULL,
	version INT NOT NULL DEFAULT 0
);
-- --------------------------------------------------------------

-- project_to_external_url
CREATE TABLE project_to_external_url
(
	project_id INT REFERENCES project (id) ON DELETE CASCADE,
	external_url_id INT REFERENCES external_url (id) ON DELETE CASCADE,
	PRIMARY KEY(project_id, external_url_id)
);
-- --------------------------------------------------------------

-- introduction_to_external_url
CREATE TABLE introduction_to_external_url
(
	introduction_id INT REFERENCES introduction (id) ON DELETE CASCADE,
	external_url_id INT REFERENCES external_url (id) ON DELETE CASCADE,
	PRIMARY KEY(introduction_id, external_url_id)
);
-- --------------------------------------------------------------

-- account
CREATE TABLE account
(
	id SERIAL PRIMARY KEY,
	login VARCHAR(256) NOT NULL UNIQUE,
	password VARCHAR(256) NOT NULL,	
	salt VARCHAR(256) NOT NULL,
	role VARCHAR(128) NOT NULL,
	version INT NOT NULL DEFAULT 0
);
-- --------------------------------------------------------------
