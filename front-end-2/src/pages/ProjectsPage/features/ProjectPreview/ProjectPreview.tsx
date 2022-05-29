import { FunctionComponent } from 'react';
import { NavLink } from 'react-router-dom';
import { ProjectPreview } from '../../../../services';
import './ProjectPreview.scss';

function createRouterLink(project: ProjectPreview): string {
  return '/project/' + project.code;
}

const ProjectPreviewComponent: FunctionComponent<{project: ProjectPreview}> = (props) => {
  return (
  <div className="container-project-preview">
    <img className={props.project.releaseDate ? 'poster' : 'poster grayscale-filter'}
         src={ props.project.posterUrl ?? '/assets/images/placeholder-wide.png' }
         alt={ props.project.posterDescription }/>
    
    <div className="content">
      <NavLink to={createRouterLink(props.project)} className="headline">
        [{ props.project.category?.displayName }] { props.project.displayName }
      </NavLink>

      <div className="description" dangerouslySetInnerHTML={{__html: props.project.description}} />
    </div>
  </div>
  );
}

export default ProjectPreviewComponent;