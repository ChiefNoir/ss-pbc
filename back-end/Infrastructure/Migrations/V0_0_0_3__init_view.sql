-- Category with total projects in that category
CREATE VIEW categories_with_projects_total_v AS
(
	SELECT 
		c.*, COALESCE(sm.total, 0) AS total_projects
	FROM 
		category c
		LEFT JOIN
		(
			SELECT
				COALESCE(category_id, (SELECT id FROM category WHERE is_everything = TRUE LIMIT 1) ) AS id, 
				COUNT(1) AS total
			FROM 
				project 
			GROUP BY
				ROLLUP(category_id)
		) sm 
	ON 
		c.id = sm.id
	ORDER BY 
		total DESC
);
-- --------------------------------------------------------------
