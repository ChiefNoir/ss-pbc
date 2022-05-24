import { useEffect, useState } from 'react';
import { ProjectPreview } from '../../services/models/_index';
import PublicApi from '../../services/PublicApi';
import { Loader } from '../../ui/_index';
import ProjectPreviewComponent from './features/ProjectPreview/ProjectPreview';
import './ProjectsPage.scss';

function ProjectsPage() {
  const [projects, setProjects] = useState<Array<ProjectPreview>>();
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    setLoading(true);
    var result = await PublicApi.getProjects(0, 100, undefined);

    setProjects(result.data.data);
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
      <div className="introduction-extension">
              {
                projects?.map
                (
                  (item: ProjectPreview) => 
                        { return ProjectPreviewComponent({project: item}) }
                )
              }
            </div>
    );
  }
}

export default ProjectsPage;