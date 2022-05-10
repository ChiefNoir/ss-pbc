import './header.scss';
import { useTranslation } from "react-i18next";
import "../../../locales/i18n";

function Header() {
  const { t } = useTranslation();
  return (
    <div className="header">
      <a>{t("header")}</a>
    </div>
  );
}

export default Header;
