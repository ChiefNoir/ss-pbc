CREATE VIEW categories_with_projects_total_v AS
(
	select 
	c.*, coalesce(sm.total, 0) as totalprojects
from 
	category c 
left join
	(
		select
			coalesce(categorycode, (select code from category where iseverything = true limit 1) ) as code, 
			count(1) as total
		from 
			project 
		group by
			rollup(categorycode)
	) sm 
on 
	c.code = sm.code
order by 
	total desc
);