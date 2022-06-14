import { useState, useEffect } from "react";
import { ErrorComponent, Loader } from "../../ui";
import { PublicApi, Introduction, Incident } from "../../services";
import { IntroductionComponent } from "./features";
import "./introduction-page.scss";

function IntroductionPage() {
  const [introduction, setIntroduction] = useState<Introduction | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [incident, setIncident] = useState<Incident | null>(null);

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      const result = await PublicApi.getIntroduction();
      if (result.data.isSucceed) {
        setIntroduction(result.data.data);
      } else {
        setIncident(result.data.error);
      }

      setLoading(false);
    };

    fetchData();
  }, []);

  if (loading) {
    return <Loader />;
  }

  if (incident) {
    return <ErrorComponent message={incident.message} detail={incident.detail}/>;
  }

  return <IntroductionComponent introduction={introduction!} />;
}

export { IntroductionPage };
