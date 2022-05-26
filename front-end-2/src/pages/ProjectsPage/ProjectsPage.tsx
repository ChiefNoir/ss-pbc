import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Category, ProjectPreview } from '../../services/models/_index';
import PublicApi from '../../services/PublicApi';
import { Loader } from '../../ui/_index';
import ButtonCategoryComponent from './features/ButtonCategory/ButtonCategoryComponent';
import ProjectPreviewComponent from './features/ProjectPreview/ProjectPreview';
import './ProjectsPage.scss';
import Pagination from '@mui/material/Pagination';
import Stack from '@mui/material/Stack';
import { useNavigate } from "react-router-dom";

function Calc(totalProjects: number): number
{
  var projectPerPage = parseInt(process.env.REACT_APP_PAGING_PROJECTS_MAX || '0') || 1;
  return Math.ceil(totalProjects / projectPerPage)
}

function ProjectsPage() {
  const [loading, setLoading] = useState(true);

  const { categoryCode }= useParams();
  const { page } = useParams();

  const [projects, setProjects] = useState<Array<ProjectPreview>>();
  const [categories, setCategories] = useState<Array<Category>>();
  const [selectedCategory, setCategory] = useState<Category>();

  let navigate = useNavigate();

  const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
    //setPage(value);

    navigate(selectedCategory?.code +'/'+ value);
  };

  const fetchData = async () => {
    setLoading(true);

    var pageNumber = parseInt(page ?? '1') || 1;
    if(pageNumber == 0)
      pageNumber = 1;
    

    var ss: Array<Category>;
    ss = categories || [];

    if(categories == undefined) {
      const categoriesResponse = await PublicApi.getCategories();
      ss = categoriesResponse.data.data;
      setCategories(ss);
    }

    if(categoryCode == undefined) {
      setCategory
      (
        ss.find(x => x.isEverything)
      );
    }
    else {
      setCategory
      (
        ss.find(x => x.code === categoryCode)
      );
    }

    const projectsResponse = await PublicApi.getProjects(pageNumber, categoryCode);

    setProjects(projectsResponse.data.data);
    setLoading(false);
  };//end of fetchData

  useEffect(() => { fetchData(); }, [categoryCode, page]);

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

        {Calc(selectedCategory?.totalProjects ?? 0) > 1 &&
          
          <div className='projects-paging'>
            <Stack spacing={2}>
              <Pagination page={ parseInt(page || '1', 10)}
                          count={ Calc(selectedCategory?.totalProjects ?? 0)} 
                          onChange= { handleChange }
                          size = "large"
                          shape="rounded" />
            </Stack>
          </div>
          
        }
      </div>
    );
  }

 
}


export default ProjectsPage;