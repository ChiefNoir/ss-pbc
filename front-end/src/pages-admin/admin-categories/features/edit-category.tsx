import * as React from "react";
import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { useEffect, useState, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";
import { Category, Incident, PrivateApi } from "../../../services";
import "./edit-category.scss";
import { Loader } from "../../../ui";

function EditCategoryDialog(props: {
  category: Category | null,
  isOpen: boolean,
  setOpen: React.Dispatch<React.SetStateAction<boolean>>,
  merge:(category: Category) => void,
  remove:(category: Category) => void}
) {
  const { t } = useTranslation();
  const [category, setCategory] = useState<Category|null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [incident, setIncident] = useState<Incident|null>(null);

  const open = props.isOpen;
  const setOpen = props.setOpen;
  const descriptionElementRef = React.useRef<HTMLElement>(null);

  useEffect(() => {
    const fetchData = async() => {
      if (open) {
        const { current: descriptionElement } = descriptionElementRef;
        if (descriptionElement !== null) {
          descriptionElement.focus();
        }

        if (props.category === null) {
          setCategory(new Category());
        } else {
          setCategory(props.category);
        }
      }
    };

    setIncident(null);
    setCategory(null);
    setIsLoading(false);
    fetchData();
  }, [open]);

  function closeDialog() {
    setOpen(false);
  };

  async function deleteCategory() {
    setIsLoading(true);

    const result = await PrivateApi.deleteCategory(category!);

    if (result.data.isSucceed) {
      props.remove(category!);
      setOpen(false);
    } else {
      setIncident(result.data.error);
    }

    setIsLoading(false);
  }

  async function saveCategory() {
    setIsLoading(true);

    const result = await PrivateApi.saveCategory(category!);

    if (result.data.isSucceed) {
      props.merge(result.data.data);
      setCategory(result.data.data);
    } else {
      setIncident(result.data.error);
    }

    setIsLoading(false);
  }

  function handleFieldChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setCategory((prevState: Category|null) => ({
      ...prevState!,
      [e.target.name]: e.target.value
    }));
  };

  return (
  <Dialog open={open} onClose={closeDialog}
          scroll="paper" fullWidth={true}
          aria-labelledby="scroll-dialog-title" aria-describedby="scroll-dialog-description">
    <DialogTitle>
      {t("Actions.Edit") + ": " + t("Category.this")}
    </DialogTitle>

    {isLoading && <Loader />}

    { incident !== null && <div className="edit-category-error"> {incident.message}</div>}

    { !isLoading && category !== null
      && <DialogContent className="container-edit-category" dividers={true}>
          <TextField label={t("Category.Code")} value={category.code ?? ""}
                     fullWidth={true} variant="outlined"
                     name="code" onChange={handleFieldChange}/>

          <TextField label={t("Category.DisplayName")} value={category.displayName ?? ""}
                     fullWidth={true} variant="outlined"
                     name="displayName" onChange={handleFieldChange}/>
        </DialogContent>
    }

    <DialogActions>
      <Button onClick={closeDialog}> {t("Actions.Cancel")} </Button>
      {(!category?.isEverything && category?.id)
        && <Button onClick={deleteCategory}> {t("Actions.Delete")} </Button>
      }
      <Button onClick={saveCategory}> {t("Actions.Save")} </Button>
    </DialogActions>
  </Dialog>
  );
};

export { EditCategoryDialog };
