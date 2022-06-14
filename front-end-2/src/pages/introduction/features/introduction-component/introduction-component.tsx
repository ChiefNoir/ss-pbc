import { FunctionComponent } from "react";
import { Introduction } from "../../../../services";
import { ButtonGlitch } from "../../../../ui";

import "./introduction-component.scss";

const IntroductionComponent: FunctionComponent<{introduction: Introduction}> = (props) => {
  return (
<div className="introduction-container">

  <div className="introduction-content">
    <h1> { props.introduction.title } </h1>
    <div className="introduction-content-description"
         dangerouslySetInnerHTML={{ __html: props.introduction.content ?? "" }}>
    </div>

    {
    props.introduction.externalUrls.length > 0
    && <div className="introduction-content-urls">
      {props.introduction.externalUrls.map(
        x => {
          return <ButtonGlitch key={x.id} displayName={x.displayName} url= {x.url} />;
        })
      }
      </div>
    }
  </div>

  <img className="introduction-poster"
         alt={ props.introduction.posterDescription }
         src={(props.introduction.posterUrl ?? "/assets/images/placeholder-tall.png")}/>

</div>
  );
};

export { IntroductionComponent };
