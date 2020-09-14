-- Category with total projects in thaat category
CREATE VIEW categories_with_projects_total_v AS
(
	select 
		c.*, coalesce(sm.total, 0) as total_projects
	from 
		site.category c
		left join
		(
			select
				coalesce(category_id, (select id from site.category where is_everything = true limit 1) ) as id, 
				count(1) as total
			from 
				site.project 
			group by
				rollup(category_id)
		) sm 
	on 
		c.id = sm.id
	order by 
		total desc
);
-- --------------------------------------------------------------