import { FunctionComponent } from 'react';
import { Link } from 'react-router-dom';
import { ProjectPreview } from '../../../../services';
import './project-preview.scss';
import { Card, CardActionArea, CardMedia, CardContent, Typography } from '@mui/material';

const ProjectPreviewComponent: FunctionComponent<{project: ProjectPreview}> = (props) => {
  const project = props.project;

  return (
  <Card sx={{ maxWidth: 300, minWidth: 300 }} >
      <CardActionArea component={Link} to={`/projects/${project.code}`} sx={{height: 400 }}>
        <CardMedia
          className='project-preview-poster'
          component="img"
          height="140"
          image={project.posterUrl} 
          alt={project.description}
        />
        <CardContent>
          <Typography className='project-preview-title' gutterBottom variant="h5" component="div">
            {project.displayName}
          </Typography>
          <Typography variant="body1" color="text.secondary">
          {project.description}
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}

export { ProjectPreviewComponent };
