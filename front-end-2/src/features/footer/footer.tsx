import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import "./footer.scss";

function Footer() {
  const { t } = useTranslation();
  return (
    <footer>
      <div className="footer-content">
        { t("Footer.Copyright") } © { new Date().getFullYear() }
      </div>
    </footer>
  );
}

export { Footer };
