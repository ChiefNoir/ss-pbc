create table News (
	Id serial primary key,
	Title varchar(64) not null,
	Content text,
	PosterUrl varchar(512) not null,
	Version int not null default 0
);

create table Category 
(
	Code varchar(128) primary key,
	DisplayName varchar(128) not null,
	IsEverything boolean not null default false,
	Version int not null default 0
);

create table Project (
	Code varchar(128) primary key,
	DisplayName varchar(128) not null,
	ReleaseDate date, 
	PosterUrl varchar(512),
	CategoryCode varchar(128) REFERENCES Category (Code),
	DescriptionShort text,
	Description text,
	Version int not null default 0
);

create table ExternalUrl (
	Id serial primary key,
	ProjectCode varchar(128) REFERENCES Project (Code),
	Url varchar(256),
	DisplayName varchar(64) not null,
	Version int not null default 0
);