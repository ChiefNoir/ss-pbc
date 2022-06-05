import "./admin-information-page.scss";
import "../../locales/i18n";
import { Information, PrivateApi } from "../../services";
import { useEffect, useState } from "react";
import { Loader } from "../../ui";
import { InformationComponent } from "./features/information-component/information-component";

function AdminInformationPage() {
  const [information, setInformation] = useState<Information>();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchInformation = async() => {
      setLoading(true);

      const result = await PrivateApi.getInformation();

      setInformation(result.data.data);
      setLoading(false);
    };

    fetchInformation();
  }, []);

  if (loading) {
    return <Loader />;
  } else {
    return <InformationComponent information={information as Information} />;
  }
}

export { AdminInformationPage };
