import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { PublicApi, Category, ProjectPreview } from '../../services';
import { Loader } from '../../ui';
import ButtonCategoryComponent from './features/ButtonCategory/ButtonCategoryComponent';
import ProjectPreviewComponent from './features/ProjectPreview/ProjectPreview';
import './ProjectsPage.scss';

import { Pagination, PaginationItem } from '@mui/material';
import { Convert, Calc } from '../../helpers'

function ProjectsPage() {
  const [loading, setLoading] = useState(true);

  const { categoryCode }= useParams();
  const { page } = useParams();

  const [projects, setProjects] = useState<Array<ProjectPreview>>();
  const [categories, setCategories] = useState<Array<Category>>();
  const [selectedCategory, setCategory] = useState<Category>();



  useEffect(() => { 
    
    const fetchData = async () => {
      setLoading(true);
  
      
  
      var tmpCategories = categories ?? [];
  
      if(categories === undefined) {
        const categoriesResponse = await PublicApi.getCategories();
        tmpCategories = categoriesResponse.data.data;
        setCategories(tmpCategories);
      }
  
      if(categoryCode === undefined) {
        setCategory(tmpCategories.find(x => x.isEverything));
      } else {
        setCategory(tmpCategories.find(x => x.code === categoryCode));
      }
  
    
      const projectsResponse = await PublicApi.getProjects(Convert.ToRestrictedNumber(page, 1), categoryCode);
  
      setProjects(projectsResponse.data.data);
      setLoading(false);
    };



    fetchData(); 
  }, [categoryCode, page]);

  if(loading)
  {
    return <Loader />;
  }
  else
  {
    return (
      <div>
        <div className='projects-categories-filter'>
        {          
          categories?.filter(x => x.totalProjects > 0).map
          (
            x =>
            {
              return <ButtonCategoryComponent key={x.id} category={x} />
            }
          )
        }
        </div>
        {
          projects?.map
          (
            x => 
            {
              return <ProjectPreviewComponent key={x.code} project={x} />
            }
          )
        }

        {Calc.Pages(selectedCategory?.totalProjects ?? 0) > 1 &&
          <div className='projects-paging'>
            <Pagination page={ Convert.ToRestrictedNumber(page, 1)}
                        count={ Calc.Pages(selectedCategory?.totalProjects ?? 0)} 
                        size = "large"
                        shape="rounded" 
                        renderItem={(item) => (
                            <PaginationItem component={Link}
                                            to={`/projects/${selectedCategory?.code}/${item.page}`}
                                            {...item} /> )} 
                />
          </div>
        }
      </div>
    );
  }

 
}


export default ProjectsPage;