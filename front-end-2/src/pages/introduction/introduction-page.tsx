import { useState, useEffect } from "react";
import "./introduction-page.scss";
import { Loader } from "../../ui";
import { PublicApi, Introduction } from "../../services";
import { IntroductionComponent } from "./features";

function IntroductionPage() {
  const [introduction, setIntroduction] = useState<Introduction>();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchIntroduction = async() => {
      setLoading(true);

      const result = await PublicApi.getIntroduction();

      setIntroduction(result.data.data);
      setLoading(false);
    };

    fetchIntroduction();
  }, []);

  if (loading) {
    return <Loader />;
  } else {
    return <IntroductionComponent introduction={introduction as Introduction} />;
  }
}

export { IntroductionPage };
