import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { PublicApi, Project } from "../../services";
import { Loader } from "../../ui";
import { ProjectComponent } from "./features";
import "./project-page.scss";

function ProjectPage() {
  const { projectCode } = useParams();

  const [project, setProject] = useState<Project>();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProject = async () => {
      setLoading(true);
  
      var result = await PublicApi.getProject(projectCode);

      setProject(result.data.data);
      setLoading(false);
    };  
    
    fetchProject();
  }, [projectCode]);

  if(loading) {
    return <Loader />;
  } else {
    return <ProjectComponent project={project as Project} />
  }
}

export { ProjectPage };
