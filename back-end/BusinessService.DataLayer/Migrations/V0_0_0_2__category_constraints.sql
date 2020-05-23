alter table category add constraint CK_category_code_reserved_keywords
    check ( lower(code) not in ('count') );

alter table category add constraint CK_category_code_only_latin_and_numbers
    check (code ~* '^[a-z0-9_.-]*$');

alter table category add constraint CK_category_code_only_lowercase
    check (code = lower(code));