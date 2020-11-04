-- Category
CREATE UNIQUE INDEX ON category (is_everything) 
    WHERE is_everything = TRUE;
-- --------------------------------------------------------------
