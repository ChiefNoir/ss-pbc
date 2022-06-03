import "./error-not-found-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";

function ErrorNotFoundPage() {
  const { t } = useTranslation();
  return (
  <div className="container-not-found">
    <div className="not-found-segment">
      <div className="not-found-glitch">
        <span className="not-found-glitch-symbol shallow"> {t("Error.PageNotFound")} </span>
        <span className="not-found-glitch-symbol deep"> {t("Error.PageNotFound")} </span>
        <span className="not-found-glitch-line"></span>
      </div>
    </div>
  </div>
  );
}

export { ErrorNotFoundPage };
