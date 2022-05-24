import { useState, useEffect }  from "react";
import "./introductionPage.scss";
import { Introduction } from "../../models/_index"
import { Loader } from "../../ui/_index"
import PublicApi from "../../services/PublicApi";
import IntroductionComponent from "./features/introductionComponent/IntroductionComponent";

function IntroductionPage() {
  const [introduction, setIntroduction] = useState<Introduction>();
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
    return <IntroductionComponent introduction={introduction as Introduction} />
  }
}

export default IntroductionPage;
