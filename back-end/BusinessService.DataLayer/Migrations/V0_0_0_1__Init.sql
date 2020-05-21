create table ExternalUrl (
	Id integer primary key,
	Url varchar(256),
	DisplayName varchar(64) not null,
	Version int not null default 0
);

create table Category 
(
	Id integer primary key,
	Code varchar(126) not null unique,
	DisplayName varchar(64) not null,
	ImageUrl varchar(126),
	IsEverything boolean not null default false,
	Version int not null default 0
);

create table News (
	Id integer primary key,
	Title varchar(64) not null,
	Text text,
	ImageUrl varchar(64) not null,
	Version int not null default 0
);

create table Project (
	Id integer primary key,
	Code varchar(126) not null unique,
	DisplayName varchar(64) not null,
	ImageUrl varchar(126),
	CategoryId int not null REFERENCES Category (Id),
	DescriptionShort text,
	Description text,
	Version int not null default 0
);

create table Project2ExternalUrl (
	Id integer primary key,
	ProjectId int not null REFERENCES Project (Id),
	ExternalUrlId int not null REFERENCES ExternalUrl (Id)
);

create table ServerSetting (
	Key varchar(64) primary key,
	DisplayName varchar(64) not null,
	Value varchar(64),
	Version int not null default 0
);