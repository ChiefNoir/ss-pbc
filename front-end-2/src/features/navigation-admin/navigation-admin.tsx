import "./navigation-admin.scss";
import { NavLink } from "react-router-dom";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";

function NavigationAdmin() {
  const { t } = useTranslation();
  return (
  <div className="navigation-admin">
      <NavLink to="/admin/intro"
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.AdminIntroduction")}
      </NavLink>
      <NavLink to="/admin/projects"
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.AdminProjects")}
      </NavLink>
      <NavLink to="/admin/users"
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.AdminUsers")}
      </NavLink>
  </div>
  );
};

export { NavigationAdmin };
