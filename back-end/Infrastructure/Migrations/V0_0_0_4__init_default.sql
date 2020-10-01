-- Categories
insert into category(code, display_name, is_everything)
	values ('all', 'Everything', true);

insert into category(code, display_name)
	values ('vg', 'Games');

insert into category(code, display_name)
	values ('ma', 'Comics');

insert into category(code, display_name)
	values ('lit', 'Stories');

insert into category(code, display_name)
	values ('bg', 'Tabletop');

insert into category(code, display_name)
	values ('s', 'Software');
-- --------------------------------------------------------------

-- Introduction
insert into introduction(title, content)
	values ('Hello', 'The service is on-line. Congratulations.');

insert into external_url (display_name, url)
	values ('GitHub', 'https://github.com/ChiefNoir');

insert into introduction_to_external_url(introduction_id, external_url_id)
	values (1, 1);
-- --------------------------------------------------------------

-- Sample project
INSERT INTO project
(
	code, 
	display_name, 
	category_id, 
	description_short, 
	description
)
values
(
	'new',
	'Brand new project',
	4,
	'The smart and short description',
	'The not smart and pretty long description'
)
--