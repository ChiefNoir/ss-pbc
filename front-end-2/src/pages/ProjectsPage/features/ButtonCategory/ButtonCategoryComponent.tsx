import { FunctionComponent } from 'react';
import { NavLink } from 'react-router-dom';
import { Category } from '../../../../services/models/_index';
import './ButtonCategoryComponent.scss';

const ButtonCategoryComponent: FunctionComponent<{category: Category}> = (props) => {
  return (
    <NavLink to='/projects' className="flat-button">
      <div className="content">
        <div className="display-name">{ props.category.displayName }</div>
        <div className="counter-container">
          <div className="conter-content">
            { props.category.totalProjects }
          </div>
        </div>
      </div>
    </NavLink>
  );
}

export default ButtonCategoryComponent;