import { FunctionComponent } from 'react';
import { useTranslation } from 'react-i18next';
import { ExternalUrl, Introduction } from '../../../../services';
import { ButtonContact } from '../_index';
import './IntroductionComponent.scss';

const IntroductionComponent: FunctionComponent<{introduction: Introduction}> = (props) => {
  const { t } = useTranslation();

  return (
  <div className="introduction-container">
    <div className="introduction-content">
      <h1 className="introduction-headline"> { props.introduction?.title } </h1>
        <hr/>
        <div className="introduction-description" 
             dangerouslySetInnerHTML={{__html: props.introduction?.content ?? ""}}>
        </div>
        
        {
          props.introduction.externalUrls.length > 0 &&
          <div>
            <h1 className="introduction-headline"> {t("Introduction.ExternalUrls")} </h1>
            <hr/>
            <div className="introduction-extension">
              {
                props.introduction.externalUrls?.map
                (
                  (item: ExternalUrl) => 
                        { return ButtonContact(item.id, item.url, item.displayName)}
                )
              }
            </div>
          </div>
        }
      </div>
      
      <img className="introduction-poster"
           alt={ props.introduction.posterDescription }
           src={(props.introduction.posterUrl ?? "/assets/images/placeholder-tall.png")}/>
    </div>
  );
}

export default IntroductionComponent;