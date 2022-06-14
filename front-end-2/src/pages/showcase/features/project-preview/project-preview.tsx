import { Link } from "react-router-dom";
import { ProjectPreview } from "../../../../services";
import { Card, CardActionArea, CardMedia, CardContent } from "@mui/material";
import "./project-preview.scss";

function ProjectPreviewComponent(props: {project: ProjectPreview}) {
  const project = props.project;

  return (
  <Card className="project-preview" >
      <CardActionArea className="project-preview-card"
                      component={Link} to={`/projects/${project.code}`}>
        <CardMedia className="project-preview-poster"
                   component="img" height="140"
                   image={project.posterUrl}
                   alt={project.displayName} />
        <CardContent>
          <h3 className="project-preview-title">
            {project.displayName}
          </h3>
          <div>
            { project.description }
          </div>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export { ProjectPreviewComponent };
