import * as React from "react";
import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle, Select, MenuItem, SelectChangeEvent, Tabs, Tab, InputLabel, FormControl } from "@mui/material";
import { useEffect, useState, FunctionComponent, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";
import { Category, ExternalUrl, PrivateApi, Project, PublicApi } from "../../../services";
import { DatePicker } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import "./edit-project.scss";
import { Loader, TabPanel } from "../../../ui";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridRowId, GridRowModel, GridToolbarContainer } from "@mui/x-data-grid";

const EditProjectDialog: FunctionComponent<
{
  projectCode: string | null,
  isOpen: boolean,
  setOpen: React.Dispatch<React.SetStateAction<boolean>>,
  merge:(project: Project) => void,
  remove:(project: Project) => void
}> = (props) => {
  const { t } = useTranslation();
  const open = props.isOpen;
  const setOpen = props.setOpen;
  const [project, setProject] = useState<Project|null>(null);
  const [categories, setCategories] = useState<Category[]>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [selectedTab, setValueSelectedTab] = React.useState(0);
  const [page, setPage] = React.useState(0);

  const [selectedFile, setSelectedFile] = useState<Blob>();

  function handleFileSelect(event: any) {
    setSelectedFile(event.target.files[0]);

    setProject((prevState: Project | null) => ({
      ...prevState!,
      posterPreview: URL.createObjectURL(event.target.files[0])
    }));
  };

  const handleClose = () => {
    setOpen(false);
  };

  function handleChangeTab(_event: React.SyntheticEvent, newValue: number) {
    setValueSelectedTab(newValue);
  };

  async function handleDelete() {
    setIsLoading(true);

    const result = await PrivateApi.deleteProject(project!);

    if (result.data.isSucceed) {
      props.remove(project!);
    }

    setOpen(false);
  }

  async function handleSave() {
    setIsLoading(true);

    const prj = project!;
    if (selectedFile) {
      const newUrl = await PrivateApi.upload(selectedFile!);

      if (newUrl.data.isSucceed) {
        prj.posterUrl = newUrl.data.data;
      }
    }

    if (prj.id !== null && prj.id < 0) {
      prj.id = null;
    }
    prj.externalUrls.forEach(x => {
      if (x.id != null && x.id < 0) {
        x.id = null;
      }
    });

    const result = await PrivateApi.saveProject(prj!);

    if (result.data.isSucceed) {
      props.merge(result.data.data);
      setProject(result.data.data);
    }

    setIsLoading(false);
  }
  const handleChangeCategory = (event: SelectChangeEvent) => {
    setProject((prevState: Project | null) => ({
      ...prevState!,
      role: event.target.value as string
    }));
  };

  const descriptionElementRef = React.useRef<HTMLElement>(null);
  useEffect(() => {
    const fetchData = async() => {
      setProject(null);
      setIsLoading(false);

      if (open) {
        const { current: descriptionElement } = descriptionElementRef;
        if (descriptionElement !== null) {
          descriptionElement.focus();
        }

        const cat = await PublicApi.getCategories();
        setCategories(cat.data.data.filter(x => !x.isEverything));

        if (props.projectCode === null) {
          const prj = new Project();

          prj.category = cat.data.data!.find(x => !x.isEverything)!;
          setProject(prj);
        } else {
          const tmp = await PublicApi.getProject(props.projectCode);
          setProject(tmp.data.data);
        }
      }
    };

    fetchData();
  }, [open]);

  function handleChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setProject((prevState: Project|null) => ({
      ...prevState!,
      [e.target.name]: e.target.value
    }));
  };

  function addExternalUrl() {
    let minId = Math.min(...project!.externalUrls.map(x => x.id!)) - 1;
    if (minId === Infinity) {
      minId = -1;
    }

    setProject((prevState: Project | null) => ({
      ...prevState!,
      externalUrls: project!.externalUrls.concat(new ExternalUrl({ id: minId }))
    }));
  };

  function deleteExternalUrl(id: GridRowId) {
    setProject((prevState: Project | null) => ({
      ...prevState!,
      externalUrls: project!.externalUrls.filter(x => x.id !== id)
    }));
  };

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ addExternalUrl } variant="text">
          { t("Admin.Add")}
        </Button>
      </GridToolbarContainer>
    );
  }

  function processRowUpdate(newRow: ExternalUrl, oldRow: ExternalUrl): ExternalUrl | Promise<ExternalUrl> {
    console.log(`oldRow: ${oldRow.displayName}`);
    console.log(`newRow: ${newRow.displayName}`);

    setProject((prevState: Project | null) => ({
      ...prevState!,
      externalUrls: project!.externalUrls.map(x => { return (x.id === oldRow.id ? newRow : x); })
    }));

    return newRow;
  }

  return (
    <div>
      <Dialog open={open} onClose={handleClose}
        scroll="paper"
        fullWidth={true}
        aria-labelledby="scroll-dialog-title"
        aria-describedby="scroll-dialog-description">
        <DialogTitle id="scroll-dialog-title">{t("Admin.Edit") + ": " + t("Project.this")} </DialogTitle>
        {isLoading
        && <Loader />
        }

        { !isLoading && project !== null
        && <DialogContent dividers={true} >

          <Tabs value={selectedTab} onChange={handleChangeTab}>
            <Tab label={t("Project.GeneralInformation")} tabIndex={0}/>
            <Tab label={t("Project.Description")} tabIndex={1}/>
            <Tab label={t("Project.Poster")} tabIndex={2}/>
            <Tab label={t("Project.ExternalUrls")} tabIndex={3}/>
          </Tabs>

          <TabPanel value={selectedTab} index={0}>
            <div className="admin-project-container">
              <TextField fullWidth={true}
                         value={project.code || ""} label={t("Project.Code")}
                         variant="outlined"
                         name="code"
                         onChange = { handleChange }/>

              <TextField fullWidth={true}
                         value={project.displayName || ""} label={t("Project.DisplayName")}
                         variant="outlined"
                         name="displayName"
                         onChange = { handleChange }/>

              <FormControl fullWidth={true}>
                <InputLabel id="demo-simple-select-label">{t("Project.Category")}</InputLabel>
                <Select value={project.category?.code}
                        variant="outlined"
                        labelId="demo-simple-select-label"
                        label={t("Project.Category")}
                        onChange= { handleChangeCategory}>
                      {categories?.map(x => {
                        return (
                        <MenuItem key={x.id} value={x.code}>
                          {x.displayName}
                        </MenuItem>);
                      })}
                </Select>
                </FormControl>
                <LocalizationProvider dateAdapter={AdapterDateFns} >
              <DatePicker

              label={t("Project.ReleaseDate")}
              value={project.releaseDate}
              onChange={(newValue) => {
                setProject((prevState: Project|null) => ({
                  ...prevState!,
                  releaseDate: newValue
                }));
              }}
        renderInput={(params) => <TextField fullWidth={true} {...params} />}
      />
</LocalizationProvider>

            </div>
          </TabPanel>

          <TabPanel value={selectedTab} index={1}>
            <div className="admin-project-container">
            <TextField fullWidth={true}
                     value={project.descriptionShort || ""} label={t("Project.DescriptionShort")} variant="outlined"
                     name="descriptionShort"
                     inputProps={{ maxLength: 280 }}
                     multiline rows= {3}
                     onChange = { handleChange }/>

              <TextField fullWidth={true}
                     value={project.description || ""} label={t("Project.Description")} variant="outlined"
                     name="description"
                     multiline rows= {10}
                     onChange = { handleChange }/>
            </div>
          </TabPanel>

          <TabPanel value={selectedTab} index={2}>
            <div className="admin-project-container">
            <img className="admin-project-poster"
                 src={project.posterPreview || project.posterUrl}/>

              <Button component="label">
                {t("Admin.Upload")} {t("Project.Poster")}
                <input type="file" hidden onChange={handleFileSelect}/>
              </Button>

              <TextField fullWidth={true}
                     value={project.posterUrl || ""} label={t("Project.PosterUrl")} variant="outlined"
                     name="posterUrl"
                     onChange = { handleChange }/>

              <TextField fullWidth={true}
                     value={project.posterDescription || ""} label={t("Project.PosterDescription")} variant="outlined"
                     name="posterDescription"
                     onChange = { handleChange }/>
            </div>
          </TabPanel>

          <TabPanel value={selectedTab} index={3}>
          <DataGrid style={{ width: "100%" }} autoHeight
          experimentalFeatures={{ newEditingApi: true }}
          processRowUpdate={processRowUpdate}
        rows={project.externalUrls}
        page={page}
        onPageChange={(newPage) => setPage(newPage)}
        pageSize={5}
        rowsPerPageOptions={[5]}
        components={{
          Toolbar: CustomToolbar
        }}
        columns={[
          { field: "displayName", flex: 0.6, headerName: t("ExternalUrl.DisplayName"), type: "string", editable: true },
          { field: "url", flex: 1, headerName: t("ExternalUrl.Url"), type: "string", editable: true },
          {
            field: "",
            headerName: t("Admin.Actions"),
            renderCell: (params: GridRenderCellParams<any, ExternalUrl, any>) => {
              return [
                <GridActionsCellItem
                showInMenu = {true}
                key={`delete-button-${params.row.id}`}
                  label="Delete"
                  onClick={() => {
                    deleteExternalUrl(params.row.id!);
                  }}
                />
              ];
            }
          }
        ]}

      />
          </TabPanel>

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

export { EditProjectDialog };
