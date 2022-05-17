import './header.scss';
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { Link } from 'react-router-dom';

function Header() {
  const { t } = useTranslation();
  return (
    <div className="header">
      <Link to='/'>{t("header")}</Link>
    </div>
  );
}

export default Header;
