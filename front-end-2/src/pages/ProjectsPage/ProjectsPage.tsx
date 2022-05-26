import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Category, ProjectPreview } from '../../services/models/_index';
import PublicApi from '../../services/PublicApi';
import { Loader } from '../../ui/_index';
import ButtonCategoryComponent from './features/ButtonCategory/ButtonCategoryComponent';
import ProjectPreviewComponent from './features/ProjectPreview/ProjectPreview';
import './ProjectsPage.scss';

function ProjectsPage() {
  const [loading, setLoading] = useState(true);

  const [projects, setProjects] = useState<Array<ProjectPreview>>();
  const [categories, setCategories] = useState<Array<Category>>();
  const { category }= useParams();
  const { page } = useParams();

  const fetchData = async () => {
    setLoading(true);

    const pageNumber = parseInt(page ?? '0') || 0;
    const projectsResponse = await PublicApi.getProjects(pageNumber, category);

    if(categories == undefined)
    {
      const categoriesResponse = await PublicApi.getCategories();
      setCategories(categoriesResponse.data.data);
    }

    setProjects(projectsResponse.data.data);
    setLoading(false);
  };

  useEffect(() => { fetchData(); }, [category, page]);

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