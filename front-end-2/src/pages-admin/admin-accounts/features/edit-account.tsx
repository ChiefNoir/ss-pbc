import * as React from "react";
import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle, Select, MenuItem, SelectChangeEvent } from "@mui/material";
import { useEffect, useState, FunctionComponent, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";
import { Account, Category, PrivateApi } from "../../../services";
import "./edit-account.scss";
import { Loader } from "../../../ui";

const EditAccountDialog: FunctionComponent<
{
  account: Account | null,
  isOpen: boolean,
  setOpen: React.Dispatch<React.SetStateAction<boolean>>,
  merge:(account: Account) => void,
  remove:(account: Account) => void
}> = (props) => {
  const { t } = useTranslation();
  const open = props.isOpen;
  const setOpen = props.setOpen;
  const [account, setAccount] = useState<Account|null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [roles, setRoles] = useState(Array<string>);

  const handleClose = () => {
    setOpen(false);
  };

  async function handleDelete() {
    setIsLoading(true);

    const result = await PrivateApi.deleteAccount(account!);

    if (result.data.isSucceed) {
      props.remove(account!);
    }

    setOpen(false);
  }

  async function handleSave() {
    setIsLoading(true);

    const result = await PrivateApi.saveAccount(account!);

    if (result.data.isSucceed) {
      props.merge(result.data.data);
      setAccount(result.data.data);
    }

    setIsLoading(false);
  }
  const handleThing = (event: SelectChangeEvent) => {
    setAccount((prevState: Account|null) => ({
      ...prevState!,
      role: event.target.value as string
    }));
  };

  const descriptionElementRef = React.useRef<HTMLElement>(null);
  useEffect(() => {
    const fetchData = async() => {
      setAccount(null);
      setIsLoading(false);

      if (open) {
        const { current: descriptionElement } = descriptionElementRef;
        if (descriptionElement !== null) {
          descriptionElement.focus();
        }
        if (roles.length === 0) {
          setIsLoading(true);
          const tmp = await PrivateApi.getRoles();
          setRoles(tmp.data.data);
          setIsLoading(false);
        }

        if (props.account === null) {
          console.log(roles);
          setAccount(new Account({ role: roles[0] }));
        } else {
          setAccount(props.account);
        }
      }
    };

    fetchData();
  }, [open]);

  function handleChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setAccount((prevState: Account|null) => ({
      ...prevState!,
      [e.target.name]: e.target.value
    }));
  };

  return (
    <div>
      <Dialog open={open} onClose={handleClose}
        scroll="paper"
        fullWidth={true}
        aria-labelledby="scroll-dialog-title"
        aria-describedby="scroll-dialog-description">
        <DialogTitle id="scroll-dialog-title">{t("Admin.Edit") + ": " + t("Account.this")} </DialogTitle>
        {isLoading
        && <Loader />
        }

        { !isLoading && account !== null
        && <DialogContent dividers={true} className="container-edit-category" >

          <TextField fullWidth={true}
                     value={account!.login} label={t("Account.Login")} variant="outlined"
                     name="login"
                     onChange = { handleChange }/>

          <TextField fullWidth={true}
                     value={account!.password || ""} label={t("Account.Password")} variant="outlined"
                     name="password"
                     onChange = { handleChange }/>
          <Select fullWidth={true} value={account!.role}
                  label={t("Account.Role")}
                  onChange= { handleThing}
                  >
            {roles?.map(x => {
              return (
            <MenuItem key={x} value={x}>
              {x ?? x}
            </MenuItem>);
            })}
            </Select>
        </DialogContent>
          }
        <DialogActions>
          <Button onClick={handleClose}> {t("Admin.Cancel")} </Button>
          <Button onClick={handleDelete}> {t("Admin.Delete")} </Button>
          <Button onClick={handleSave}> {t("Admin.Save")} </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export { EditAccountDialog };
