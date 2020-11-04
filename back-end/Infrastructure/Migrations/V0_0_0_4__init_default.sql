-- Categories
INSERT INTO category(code, display_name, is_everything)
	VALUES ('all', 'Everything', TRUE);

INSERT INTO category(code, display_name)
	VALUES ('vg', 'Games');

INSERT INTO category(code, display_name)
	VALUES ('ma', 'Comics');

INSERT INTO category(code, display_name)
	VALUES ('lit', 'Stories');

INSERT INTO category(code, display_name)
	VALUES ('bg', 'Tabletop');

INSERT INTO category(code, display_name)
	VALUES ('s', 'Software');
-- --------------------------------------------------------------

-- Introduction
INSERT INTO introduction(title, content)
	VALUES ('Hello', 'The service is on-line. Congratulations.');

INSERT INTO external_url (display_name, url)
	VALUES ('GitHub', 'https://github.com/ChiefNoir');

INSERT INTO introduction_to_external_url(introduction_id, external_url_id)
	VALUES (1, 1);
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
VALUES
(
	'placeholder_code',
	'Brand new project',
	6,
	'The smart and short description.',
	'Not that smart and pretty long description.'
);

INSERT INTO external_url (display_name, url)
	VALUES ('GitHub', 'https://github.com/ChiefNoir');

INSERT INTO project_to_external_url(project_id, external_url_id)
	VALUES (1, 2);

INSERT INTO gallery_image (extra_url, image_url, project_id)
	VALUES (NULL, 'https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png', 1);
-- --------------------------------------------------------------
