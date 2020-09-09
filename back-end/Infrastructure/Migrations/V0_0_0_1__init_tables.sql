create table introduction 
(
	id serial primary key,
	title text,
	poster_url varchar(512),	
	poster_description varchar(512),
	content text,
	version int not null default 0
);

create table category 
(
	id serial primary key,
	code varchar(128) not null UNIQUE,
	display_name varchar(128) not null,
	is_everything boolean not null default false,
	version int not null default 0
);

create table project (
	id serial primary key,
	code varchar(128) not null UNIQUE,
	display_name varchar(128) not null,
	release_date date, 
	poster_url varchar(512),
	poster_description varchar(512),
	category_id int REFERENCES category (id),
	description_short text,
	Description text,
	Version int not null default 0
);

create table gallery_image (
	id serial primary key,

	extra_url varchar(256),
	image_url varchar(256) not null,

	project_id int REFERENCES project (id) on delete cascade,
	version int not null default 0
);

create table external_url (
	id serial primary key,
	url varchar(256),
	display_name varchar(64) not null,
	version int not null default 0
);

create table project_to_external_url (
	project_id int REFERENCES project (id) on delete cascade,
	external_url_id int REFERENCES external_url (id) on delete cascade,
	PRIMARY KEY(project_id, external_url_id)
);

create table introduction_to_external_url (
	introduction_id int REFERENCES introduction (id) on delete cascade,
	external_url_id int REFERENCES external_url (id) on delete cascade,
	PRIMARY KEY(introduction_id, external_url_id)
);

create table account (
	id serial primary key,
	login varchar(256) not null UNIQUE,
	password varchar(256) not null,	
	salt varchar(256) not null,
	role varchar(128) not null,
	version int not null default 0
);