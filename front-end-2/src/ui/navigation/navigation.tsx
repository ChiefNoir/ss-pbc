import './navigation.scss';
import { useTranslation } from "react-i18next";

import "../../locales/i18n";
import { NavLink } from 'react-router-dom';

function Navigation() {
  const { t } = useTranslation();
  return (
  <div className="navigation">
      <NavLink to='/'
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.Home")}
      </NavLink>

      <NavLink to='/projects'
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.Projects")}
      </NavLink>
  </div>
  )
};

export default Navigation;
