CREATE VIEW categories_with_projects_total_v AS
(
	select 
	c.*, coalesce(sm.total, 0) as total_projects
from 
	category c 
left join
	(
		select
			coalesce(category_code, (select code from category where is_everything = true limit 1) ) as code, 
			count(1) as total
		from 
			project 
		group by
			rollup(category_code)
	) sm 
on 
	c.code = sm.code
order by 
	total desc
);