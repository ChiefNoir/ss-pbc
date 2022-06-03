import "./navigation.scss";
import { NavLink } from "react-router-dom";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";

function Navigation() {
  const { t } = useTranslation();
  return (
  <div className="navigation">
      <NavLink to="/"
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.Home")}
      </NavLink>

      <NavLink to="/projects"
               className={({ isActive }) => (isActive ? " active" : "")}>
        {t("Navigation.Projects")}
      </NavLink>
  </div>
  );
};

export { Navigation };
