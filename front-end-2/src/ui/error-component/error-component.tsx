import { useTranslation } from "react-i18next";
import "./error-component.scss";

function ErrorComponent(props: {message: string, detail: string | null}) {
  const { t } = useTranslation();

  return (
  <div className="container-error">
    <h2> {props.message}</h2>
    <h3> {props.detail}</h3>
    <h4> {t("Messages.ReloadPage")} </h4>
  </div>
  );
};

export { ErrorComponent };
