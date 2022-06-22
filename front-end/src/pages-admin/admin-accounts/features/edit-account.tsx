import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle, Select, MenuItem, SelectChangeEvent } from "@mui/material";
import { useEffect, useState, ChangeEvent, Dispatch, SetStateAction, useRef } from "react";
import { useTranslation } from "react-i18next";
import { Account, Incident, PrivateApi } from "../../../services";
import "./edit-account.scss";
import { Loader } from "../../../ui";

function EditAccountDialog(props: {
  account: Account | null,
  isOpen: boolean,
  setOpen: Dispatch<SetStateAction<boolean>>,
  mergeAccount:(account: Account) => void,
  removeAccount:(account: Account) => void}
) {
  const { t } = useTranslation();
  const [account, setAccount] = useState<Account|null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [incident, setIncident] = useState<Incident|null>(null);
  const [roles, setRoles] = useState<string[]>([]);

  const open = props.isOpen;
  const setOpen = props.setOpen;
  const mergeAccount = props.mergeAccount;
  const removeAccount = props.removeAccount;
  const descriptionElementRef = useRef<HTMLElement>(null);

  useEffect(() => {
    async function fetchData() {
      if (open) {
        const { current: descriptionElement } = descriptionElementRef;
        if (descriptionElement !== null) {
          descriptionElement.focus();
        }

        let tmpRoles = roles ?? [];
        if (tmpRoles.length === 0) {
          setIsLoading(true);
          const rolesResponse = await PrivateApi.getRoles();
          if (rolesResponse.isSucceed) {
            tmpRoles = rolesResponse.data;
            setRoles(rolesResponse.data);
          } else {
            setIncident(rolesResponse.error);
            setIsLoading(false);
            return;
          }
        }

        if (props.account === null) {
          setAccount(new Account({ role: tmpRoles[1] }));
        } else {
          setAccount(props.account);
        }

        setIsLoading(false);
      }
    }

    setIncident(null);
    setAccount(null);
    setIsLoading(false);

    fetchData();
  }, [open]);

  function closeDialog() {
    setOpen(false);
  };

  async function deleteAccount() {
    setIncident(null);
    setIsLoading(true);

    const result = await PrivateApi.deleteAccount(account!);

    if (result.isSucceed) {
      removeAccount(account!);
      setOpen(false);
    } else {
      setIncident(result.error);
      setIsLoading(false);
    }
  }

  async function saveAccount() {
    setIncident(null);
    setIsLoading(true);

    const result = await PrivateApi.saveAccount(account!);
    if (result.isSucceed) {
      mergeAccount(result.data);
      setAccount(result.data);
    } else {
      setIncident(result.error);
    }

    setIsLoading(false);
  }

  function handleRoleChange(event: SelectChangeEvent) {
    setAccount((prevState: Account|null) => ({
      ...prevState!,
      role: event.target.value as string
    }));
  };

  function handleFieldChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setAccount((prevState: Account|null) => ({
      ...prevState!,
      [e.target.name]: e.target.value
    }));
  };

  return (
  <div>
    <Dialog open={open} onClose={closeDialog}
            scroll="paper" fullWidth={true}>
      <DialogTitle>
          {t("Actions.Edit") + ": " + t("Account.this")}
      </DialogTitle>

      { incident !== null && <div className="edit-account-error"> {incident.message}</div>}

      { isLoading && <Loader />}

      { !isLoading && account !== null
      && <DialogContent dividers={true} className="container-edit-category">
          <TextField label={t("Account.Login")} fullWidth={true} variant="outlined"
                     value={account.login} name="login"
                     onChange={handleFieldChange}/>

          <TextField label={t("Account.Password")} fullWidth={true} variant="outlined"
                     value={account.password || ""} name="password"
                     onChange={handleFieldChange }/>

          <Select label={t("Account.Role")} fullWidth={true}
                  value={account.role}
                  onChange= {handleRoleChange}>
              {roles?.map(x => {
                return (<MenuItem key={`role-id-${x}`} value={x}> {x} </MenuItem>);
              })}
          </Select>
        </DialogContent>
      }

      <DialogActions>
        <Button onClick={closeDialog}> {t("Actions.Cancel")} </Button>
        <Button onClick={deleteAccount}> {t("Actions.Delete")} </Button>
        <Button onClick={saveAccount}> {t("Actions.Save")} </Button>
      </DialogActions>
    </Dialog>
  </div>
  );
};

export { EditAccountDialog };
