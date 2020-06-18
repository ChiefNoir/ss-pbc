create table news (
	id serial primary key,
	title varchar(64) not null,
	content text,
	poster_url varchar(512) not null,
	poster_description varchar(512),
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

create table Project (
	id serial primary key,
	code varchar(128) not null UNIQUE,
	display_name varchar(128) not null,
	release_date date, 
	poster_url varchar(512) not null,
	poster_description varchar(512),
	category_id int REFERENCES category (id),
	description_short text,
	Description text,
	Version int not null default 0
);

create table external_url (
	id serial primary key,
	project_id int REFERENCES project (id),
	url varchar(256),
	display_name varchar(64) not null,
	version int not null default 0
);

create table account (
	id serial primary key,
	login varchar(256) not null UNIQUE,
	password varchar(256) not null,	
	salt varchar(256) not null,
	role varchar(128) not null,
	version int not null default 0
);