import { useState, useEffect }  from "react";
import "./introductionPage.scss";
import { useTranslation } from "react-i18next";
import { Introduction } from "../../models/_index"
import { Loader, ButtonContact, Navigation } from "../../ui/_index"
import PublicApi from "../../services/PublicApi";

function IntroductionPage() {
  const { t } = useTranslation();
  const [introduction, setIntroduction] = useState<Introduction | null>(null);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
      setLoading(true);
      var result = await PublicApi.getIntroduction();

      setIntroduction(result.data.data);
      setLoading(false);
    };

  useEffect(() => { fetchData(); }, []);

  if(loading)
  {
    return <Loader />;
  }
  else
  {
    return (
    <div className="introduction-container">
      <div className="introduction-content">
        <h1 className="introduction-headline"> { introduction?.title } </h1>
        <hr/>
        <div className="introduction-description" 
             dangerouslySetInnerHTML={{__html: introduction?.content || ""}}>
        </div>
        
        {
        (introduction?.externalUrls?.length ?? 0) > 0 &&
        <div>
          <h1 className="introduction-headline"> {t("Introduction.ExternalUrls")} </h1>
          <hr/>
          <div className="introduction-extension">
            { introduction?.externalUrls.map
              (object => 
                { return ButtonContact(object.id, object.url, object.displayName)})
            }
          </div>
        </div>
        }
      </div>
      
      <img className="introduction-poster"
           alt={ introduction?.posterDescription}
           src={(introduction?.posterUrl ?? "/assets/images/placeholder-tall.png")}/>
    </div>
    );
  }
}

export default IntroductionPage;
