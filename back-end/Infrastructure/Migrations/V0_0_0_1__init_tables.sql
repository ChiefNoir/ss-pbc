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
	code varchar(128) primary key,
	display_name varchar(128) not null,
	is_everything boolean not null default false,
	version int not null default 0
);

create table Project (
	code varchar(128) primary key,
	display_name varchar(128) not null,
	release_date date, 
	poster_url varchar(512) not null,
	poster_description varchar(512),
	category_code varchar(128) REFERENCES category (code),
	description_short text,
	Description text,
	Version int not null default 0
);

create table external_url (
	id serial primary key,
	project_code varchar(128) REFERENCES project (code),
	url varchar(256),
	display_name varchar(64) not null,
	version int not null default 0
);