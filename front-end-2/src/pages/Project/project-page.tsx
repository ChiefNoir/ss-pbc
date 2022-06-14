import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { PublicApi, Project, Incident } from "../../services";
import { ErrorComponent, Loader } from "../../ui";
import { ProjectComponent } from "./features";
import "./project-page.scss";

function ProjectPage() {
  const { projectCode } = useParams();

  const [project, setProject] = useState<Project>();
  const [loading, setLoading] = useState(true);
  const [incident, setIncident] = useState<Incident | null>(null);

  useEffect(() => {
    const fetchProject = async() => {
      setLoading(true);

      const result = await PublicApi.getProject(projectCode);
      if (result.data.isSucceed) {
        setProject(result.data.data);
      } else {
        setIncident(result.data.error);
      }

      setLoading(false);
    };

    fetchProject();
  }, [projectCode]);

  if (loading) {
    return <Loader />;
  }

  if (incident) {
    return <ErrorComponent message={incident.message} detail={incident.detail}/>;
  }

  return <ProjectComponent project={project!} />;
}

export { ProjectPage };
