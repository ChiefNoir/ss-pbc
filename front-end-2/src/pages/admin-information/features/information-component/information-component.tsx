import { FunctionComponent } from "react";
import { useTranslation } from "react-i18next";
import { Information } from "../../../../services";
import "./information-component.scss";

const InformationComponent: FunctionComponent<{information: Information}> = (props) => {
  const { t } = useTranslation();
  const info = props.information;
  return (
<div className="information-container">
  <h1>{ t("Information.Title") }</h1>
  <div className="information-item"><b>{ t("Information.ApiVersion") } </b>{info.apiVersion}</div>
</div>
  );
};

export { InformationComponent };
