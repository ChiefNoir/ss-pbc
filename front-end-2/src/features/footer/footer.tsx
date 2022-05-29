import "./footer.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";

function Footer() {
  const { t } = useTranslation();
  return (
    <footer>{ t("Footer.Copyright") } © { new Date().getFullYear() }</footer>
  );
}

export default Footer;
