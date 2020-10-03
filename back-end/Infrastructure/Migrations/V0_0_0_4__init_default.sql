-- Categories
insert into category(id, code, display_name, is_everything)
	values (1, 'all', 'Everything', true);

insert into category(id, code, display_name)
	values (2, 'vg', 'Games');

insert into category(id, code, display_name)
	values (3, 'ma', 'Comics');

insert into category(id, code, display_name)
	values (4, 'lit', 'Stories');

insert into category(id, code, display_name)
	values (5, 'bg', 'Tabletop');

insert into category(id, code, display_name)
	values (6, 's', 'Software');
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
insert into project
(
	id,
	code, 
	display_name, 
	category_id, 
	description_short, 
	description
)
values
(
	1,
	'placeholder_code',
	'Brand new project',
	6,
	'The smart and short description.',
	'Not that smart and pretty long description.'
);

insert into external_url (display_name, url)
	values ('GitHub', 'https://github.com/ChiefNoir');

insert into project_to_external_url(project_id, external_url_id)
	values (1, 2);
-- --------------------------------------------------------------