import { Introduction } from "../../../../services";
import { ButtonGlitch } from "../../../../ui";
import "./introduction-component.scss";

function IntroductionComponent(props: {introduction: Introduction}) {
  const intro = props.introduction;

  return (
  <div className="introduction-container">
    <div className="introduction-content">
      <h1> { intro.title } </h1>
      <div className="introduction-content-description"
           dangerouslySetInnerHTML={{ __html: intro.content ?? "" }}>
      </div>

      <div className="introduction-content-urls">
        {intro.externalUrls.map(
          x => {
            return <ButtonGlitch key={`external-url-btn-${x.id}`}
                                 displayName={x.displayName}
                                 url={x.url} />;
          })
        }
        </div>
    </div>

    <img className="introduction-poster"
         alt={intro.posterDescription}
         src={(intro.posterUrl ?? "/assets/images/placeholder.png")}/>
  </div>
  );
};

export { IntroductionComponent };
