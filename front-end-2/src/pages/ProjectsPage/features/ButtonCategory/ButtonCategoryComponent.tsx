import { FunctionComponent } from 'react';
import { NavLink } from 'react-router-dom';
import { Category } from '../../../../services/models/_index';
import './ButtonCategoryComponent.scss';

const ButtonCategoryComponent: FunctionComponent<{category: Category}> = (props) => {
  return (
    <NavLink to={'/projects/'+ props.category.code } className="button-contact">
      { props.category.displayName } [{ props.category.totalProjects }]
    </NavLink>
  );
}

export default ButtonCategoryComponent;