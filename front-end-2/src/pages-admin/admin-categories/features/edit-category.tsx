import * as React from "react";
import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { useEffect, useState, FunctionComponent, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";
import { Category, PrivateApi } from "../../../services";
import "./edit-category.scss";
import { Loader } from "../../../ui";

const EditCategoryDialog: FunctionComponent<
{
  category: Category | null,
  isOpen: boolean,
  setOpen: React.Dispatch<React.SetStateAction<boolean>>,
  merge:(category: Category) => void,
  remove:(category: Category) => void
}> = (props) => {
  const { t } = useTranslation();
  const open = props.isOpen;
  const setOpen = props.setOpen;
  const [category, setCategory] = useState<Category|null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleClose = () => {
    setOpen(false);
  };

  async function handleDelete() {
    setIsLoading(true);

    const result = await PrivateApi.deleteCategory(category!);

    if (result.data.isSucceed) {
      props.remove(category!);
    }

    setOpen(false);
  }

  async function handleSave() {
    setIsLoading(true);

    const result = await PrivateApi.saveCategory(category!);

    if (result.data.isSucceed) {
      props.merge(result.data.data);
      setCategory(result.data.data);
    }

    setIsLoading(false);
  }

  const descriptionElementRef = React.useRef<HTMLElement>(null);
  useEffect(() => {
    const fetchData = async() => {
      setCategory(null);
      setIsLoading(false);

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

    fetchData();
  }, [open]);

  function handleChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setCategory((prevState: Category|null) => ({
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
        <DialogTitle id="scroll-dialog-title">{t("Admin.Edit") + ": " + t("Category.this")} </DialogTitle>
        {isLoading
        && <Loader />
        }

        { !isLoading && category !== null
        && <DialogContent dividers={true} className="container-edit-category" >

          <TextField fullWidth={true} value = {category!.code} label= {t("Category.Code")} variant="outlined"
                 name="code"
                 onChange = { handleChange }/>
        <TextField fullWidth={true} value = {category!.displayName} label= {t("Category.DisplayName")} variant="outlined"
                 name="displayName"
                 onChange = { handleChange }/>
        </DialogContent>
          }
        <DialogActions>
          <Button onClick={handleClose}> {t("Actions.Cancel")} </Button>
          {(!category?.isEverything && category?.id)
          && <Button onClick={handleDelete}> {t("Actions.Delete")} </Button>
          }
          <Button onClick={handleSave}> {t("Actions.Save")} </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export { EditCategoryDialog };
