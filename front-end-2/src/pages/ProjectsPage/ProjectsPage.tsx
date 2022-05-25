import { useEffect, useState } from 'react';
import { Category, ProjectPreview } from '../../services/models/_index';
import PublicApi from '../../services/PublicApi';
import { Loader } from '../../ui/_index';
import ButtonCategoryComponent from './features/ButtonCategory/ButtonCategoryComponent';
import ProjectPreviewComponent from './features/ProjectPreview/ProjectPreview';
import './ProjectsPage.scss';

function ProjectsPage() {
  const [projects, setProjects] = useState<Array<ProjectPreview>>();
  const [categories, setCategories] = useState<Array<Category>>();
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    setLoading(true);
    const projectsResponse = await PublicApi.getProjects(0, 100, undefined);
    const categoriesResponse = await PublicApi.getCategories();

    setProjects(projectsResponse.data.data);
    setCategories(categoriesResponse.data.data);

    setLoading(false);
  };

  useEffect(() => { fetchData(); }, []);

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
            (item: Category) =>
            {
              return <ButtonCategoryComponent key={item.id} category={item} />
            }
          )
        }
        </div>
        {
          projects?.map
          (
            (item: ProjectPreview) => 
            {
              return <ProjectPreviewComponent key={item.code} project={item} />
            }
          )
        }
      </div>
    );
  }
}

export default ProjectsPage;