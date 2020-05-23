alter table category add constraint CK_category_reserved_keywords
    CHECK ( lower(code) not in ('count'))