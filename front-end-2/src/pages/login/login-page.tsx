import { Button, TextField } from "@mui/material";
import "./login-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { PrivateApi } from "../../services/PrivateApi";
import { ChangeEvent, useState } from "react";
import { Credentials } from "../../services";
import { saveIdentity } from "../../storage";
import { useDispatch } from "react-redux";

function LoginPage() {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [credentials, setCred] = useState<Credentials>(new Credentials());

  function handleChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setCred((prevState: Credentials) => ({
      ...prevState,
      [e.target.name]: e.target.value
    }));
  };

  const doLogin = async() => {
    setLoading(true);
    const result = await PrivateApi.login(credentials as Credentials);

    if (result.data.isSucceed) {
      dispatch(saveIdentity(result.data.data));
    } else {
      setError(result.data.error.message);
      setLoading(false);
    }
  };

  return (
    <div className={`container-login ${loading ? "disabled" : ""}`} aria-disabled = {loading}>
      <h1> {t("Admin.Credentials")} </h1>
      <b> { error } </b>
      <TextField value = {credentials.login} label= {t("Admin.Login")} variant="outlined"
                 name="login"
                 onChange = { handleChange }/>
      <TextField value={credentials.password} label= {t("Admin.Password")} type="password" variant="outlined"
                 name="password"
                 onChange = { handleChange }/>
      <Button onClick={doLogin} variant="outlined"> {t("Admin.DoLogin")} </Button>
    </div>
  );
}

export { LoginPage };
