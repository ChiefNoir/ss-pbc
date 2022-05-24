import './navigation.scss';
import { useTranslation } from "react-i18next";

import "../../locales/i18n";
import { Link } from 'react-router-dom';

function Navigation() {
  const { t } = useTranslation();
  return (
  <div className="navigation">
      <Link to='/'>{t("Navigation.Home")}</Link>
      <Link to='/projects'>{t("Navigation.Projects")}</Link>
      <Link to='/login'>{t("Navigation.Login")}</Link>
  </div>
  )
};

export default Navigation;
