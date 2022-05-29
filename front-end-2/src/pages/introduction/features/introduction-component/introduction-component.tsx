import { FunctionComponent } from "react";
import { useTranslation } from "react-i18next";
import { Introduction } from "../../../../services";

import "./introduction-component.scss";

const IntroductionComponent: FunctionComponent<{introduction: Introduction}> = (props) => {
  const { t } = useTranslation();

  return (
  <div className="container-introduction">

    <div className="container-introduction-content">
      <h1> { props.introduction.title } </h1>
      <hr/>
      <div className="container-introduction-content-description" 
           dangerouslySetInnerHTML={{__html: props.introduction.content ?? ""}}>
      </div>

      {props.introduction.externalUrls.length > 0 &&
      <div>
        <h1> {t("Introduction.ExternalUrls")} </h1>
        <hr/>
        <div className="container-introduction-content-extension">
          {props.introduction.externalUrls.map(
            x => { 
              return <div></div>// ButtonContact(x.id, x.url, x.displayName)
            }
              )
          }
        </div>
      </div>}
    </div>

    <img className="container-introduction-poster"
         alt={ props.introduction.posterDescription }
         src={(props.introduction.posterUrl ?? "/assets/images/placeholder-tall.png")}/>
  </div>
  );
}

export { IntroductionComponent };