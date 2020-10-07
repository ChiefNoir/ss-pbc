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
insert into project
(
	code, 
	display_name, 
	category_id, 
	description_short, 
	description
)
values
(
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

insert into gallery_image (extra_url, image_url, project_id)
	values (null, 'https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png', 1);
-- --------------------------------------------------------------