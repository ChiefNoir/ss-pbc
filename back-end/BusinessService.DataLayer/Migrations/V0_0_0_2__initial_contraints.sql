-- Category
alter table category add constraint CK_category_code__reserved_keywords
    check ( lower(code) not in ('count') );

alter table category add constraint CK_category_code__only_latin_and_numbers
    check (code ~* '^[a-z0-9_.-]*$');

alter table category add constraint CK_category_code__only_lowercase
    check (code = lower(code));

create unique index on category (IsEverything) 
    where IsEverything = true;

-- --------------------------------------------------------------

-- Project
alter table project add constraint CK_project_code__reserved_keywords
    check ( lower(code) not in ('count') );

alter table project add constraint CK_project_code__only_latin_and_numbers
    check (code ~* '^[a-z0-9_.-]*$');

alter table project add constraint CK_project_code__only_lowercase
    check (code = lower(code));
-- --------------------------------------------------------------