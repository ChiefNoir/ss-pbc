import { Button, TextField } from "@mui/material";
import "./login-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { PrivateApi } from "../../services/PrivateApi";
import { ChangeEvent, useState } from "react";
import { Credentials, Incident } from "../../services";
import { saveIdentity, deleteIdentity, store } from "../../storage";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { NavigationAdmin } from "../../features";

function LoginPage() {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const [loading, setLoading] = useState(false);
  const [incident, setIncident] = useState<Incident | null>(null);
  const [credentials, setCred] = useState<Credentials>(new Credentials());

  const navigate = useNavigate();

  function handleChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setCred((prevState: Credentials) => ({
      ...prevState,
      [e.target.name]: e.target.value
    }));
  };

  async function doLogin() {
    setLoading(true);
    const result = await PrivateApi.login(credentials as Credentials);

    if (result.data.isSucceed) {
      dispatch(saveIdentity(result.data.data));
      navigate("/admin");
    } else {
      setIncident(result.data.error);
      setLoading(false);
    }
  };

  async function doLogout() {
    dispatch(deleteIdentity());
    navigate("/");
  }

  if (store.getState().identity.value) {
    return (
      <div className="container-login">
        <NavigationAdmin />
        <b>{t("Message.LoggedIn")}</b>
        <Button onClick={doLogout} variant="outlined"> {t("Actions.DoLogout")} </Button>
      </div>
    );
  }

  return (
    <div className={`container-login ${loading ? "disabled" : ""}`}
         aria-disabled = {loading}>
      <h1> {t("Credentials.this")} </h1>
      <b> { incident?.message } </b>
      <TextField label= {t("Credentials.Login")}
                 value = {credentials.login}
                 name="login" variant="outlined"
                 onChange = { handleChange }/>
      <TextField label= {t("Credentials.Password")}
                 value={credentials.password}
                 name="password" type="password" variant="outlined"
                 onChange = { handleChange }/>
      <Button onClick={doLogin} variant="outlined"> {t("Actions.DoLogin")} </Button>
    </div>
  );
}

export { LoginPage };
