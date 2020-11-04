-- Category
ALTER TABLE category ADD CONSTRAINT CK_category_code__only_latin_and_numbers
    CHECK (code ~* '^[a-z0-9_.-]*$');

ALTER TABLE category ADD CONSTRAINT CK_category_code__only_lowercase
    CHECK (code = LOWER(code));
-- --------------------------------------------------------------

-- Project
ALTER TABLE project ADD CONSTRAINT CK_project_code__only_latin_and_numbers
    CHECK (code ~* '^[a-z0-9_.-]*$');

ALTER TABLE project ADD CONSTRAINT CK_project_code__only_lowercase
    CHECK (code = LOWER(Code));
-- --------------------------------------------------------------
