import * as React from "react";
import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle, Select, MenuItem, SelectChangeEvent, Tabs, Tab, InputLabel, FormControl } from "@mui/material";
import { useEffect, useState, ChangeEvent, Dispatch } from "react";
import { useTranslation } from "react-i18next";
import { Category, ExternalUrl, Incident, PrivateApi, Project, PublicApi } from "../../../services";
import { DatePicker } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import "./edit-project.scss";
import { Loader, TabPanel } from "../../../ui";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridRowId, GridToolbarContainer } from "@mui/x-data-grid";

function EditProjectDialog(props:
{
  projectCode: string | null,
  isOpen: boolean,
  setOpen: Dispatch<React.SetStateAction<boolean>>,
  merge:(project: Project) => void,
  remove:(project: Project) => void
}) {
  const { t } = useTranslation();
  const [project, setProject] = useState<Project|null>(null);
  const [categories, setCategories] = useState<Category[]>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [selectedTab, setValueSelectedTab] = React.useState(0);
  const [page, setPage] = React.useState(0);
  const [selectedFile, setSelectedFile] = useState<Blob>();
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

        const cat = await PublicApi.getCategories();
        if (cat.data.isSucceed) {
          setCategories(cat.data.data.filter(x => !x.isEverything));
        } else {
          setIncident(cat.data.error);
          setIsLoading(false);
          return;
        }

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

    setProject(null);
    setIncident(null);
    setIsLoading(true);

    fetchData();
    setIsLoading(false);
  }, [open]);

  function handleFileSelect(event: any) {
    setSelectedFile(event.target.files[0]);

    setProject((prevState: Project | null) => ({
      ...prevState!,
      posterPreview: URL.createObjectURL(event.target.files[0])
    }));
  };

  function handleClose() {
    setOpen(false);
  };

  function handleChangeTab(_event: React.SyntheticEvent, newValue: number) {
    setValueSelectedTab(newValue);
  };

  async function deleteProject() {
    setIsLoading(true);
    setIncident(null);

    const result = await PrivateApi.deleteProject(project!);

    if (result.data.isSucceed) {
      props.remove(project!);
      setOpen(false);
    } else {
      setIncident(result.data.error);
      setIsLoading(false);
    }
  }

  async function saveProject() {
    setIsLoading(true);
    setIncident(null);

    const prj = project!;
    if (selectedFile) {
      const newUrl = await PrivateApi.upload(selectedFile!);

      if (newUrl.data.isSucceed) {
        prj.posterUrl = newUrl.data.data;
      } else {
        setIncident(newUrl.data.error);
        setIsLoading(false);
        return;
      }
    }

    const result = await PrivateApi.saveProject(prj!);
    if (result.data.isSucceed) {
      props.merge(result.data.data);
      setProject(result.data.data);
    } else {
      setIncident(result.data.error);
      setIsLoading(false);
      return;
    }

    setIsLoading(false);
  }

  function handleChangeCategory(event: SelectChangeEvent) {
    setProject((prevState: Project | null) => ({
      ...prevState!,
      role: event.target.value as string
    }));
  };

  function handleFieldChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setProject((prevState: Project|null) => ({
      ...prevState!,
      [e.target.name]: e.target.value
    }));
  };

  function handleRowUpdate(newRow: ExternalUrl, oldRow: ExternalUrl): ExternalUrl | Promise<ExternalUrl> {
    setProject((prevState: Project | null) => ({
      ...prevState!,
      externalUrls: project!.externalUrls.map(x => { return (x.id === oldRow.id ? newRow : x); })
    }));

    return newRow;
  }

  function addExternalUrl() {
    let minId = Math.min(...project!.externalUrls.map(x => x.id!)) - 1;
    if (minId === Infinity || minId === 0) {
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
          { t("Actions.Add")}
        </Button>
      </GridToolbarContainer>
    );
  }

  return (
  <Dialog open={open} onClose={handleClose} fullWidth={true}
          aria-labelledby="scroll-dialog-title" aria-describedby="scroll-dialog-description">
    <DialogTitle>{`${t("Actions.Edit")}: ${t("Project.this")}`}</DialogTitle>
      {isLoading && <Loader /> }

      {incident !== null && <div className="edit-category-error"> {incident.message}</div>}

      {!isLoading && project !== null
      && <DialogContent dividers={true}>
          <Tabs value={selectedTab} onChange={handleChangeTab}>
            <Tab label={t("Project.GeneralInformation")} tabIndex={0}/>
            <Tab label={t("Project.Description")} tabIndex={1}/>
            <Tab label={t("Project.Poster")} tabIndex={2}/>
            <Tab label={t("Project.ExternalUrls")} tabIndex={3}/>
          </Tabs>

          <TabPanel value={selectedTab} index={0}>
            <div className="admin-project-container">
              <TextField value={project.code || ""} label={t("Project.Code")}
                         variant="outlined" fullWidth={true}
                         name="code" onChange = { handleFieldChange }/>

              <TextField value={project.displayName || ""} label={t("Project.DisplayName")}
                         variant="outlined" fullWidth={true}
                         name="displayName" onChange = { handleFieldChange }/>

              <FormControl fullWidth={true}>
                <InputLabel>{t("Project.Category")}</InputLabel>
                <Select value={project.category?.code} label={t("Project.Category")}
                        variant="outlined"
                        onChange= {handleChangeCategory}>
                      {categories?.map(x => {
                        return (
                        <MenuItem key={x.id} value={x.code}>
                          {x.displayName}
                        </MenuItem>);
                      })}
                </Select>
              </FormControl>

              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <DatePicker value={project.releaseDate} label={t("Project.ReleaseDate")}
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
              <TextField value={project.descriptionShort || ""} label={t("Project.DescriptionShort")}
                         fullWidth={true} variant="outlined"
                         multiline rows={3} inputProps={{ maxLength: 280 }}
                         name="descriptionShort" onChange = { handleFieldChange }/>

              <TextField value={project.description || ""} label={t("Project.Description")}
                         fullWidth={true} variant="outlined"
                         multiline rows= {10}
                         name="description" onChange={handleFieldChange}/>
            </div>
          </TabPanel>

          <TabPanel value={selectedTab} index={2}>
            <div className="admin-project-container">
              <img className="admin-project-poster"
                   src={project.posterPreview || project.posterUrl}/>

              <Button component="label">
                {t("Actions.Upload")} {t("Project.Poster")}
                <input type="file" hidden onChange={handleFileSelect}/>
              </Button>

              <TextField value={project.posterUrl || ""} label={t("Project.PosterUrl")}
                         fullWidth={true} variant="outlined"
                         name="posterUrl" onChange={handleFieldChange}/>

              <TextField value={project.posterDescription || ""} label={t("Project.PosterDescription")}
                         fullWidth={true} variant="outlined"
                         name="posterDescription" onChange = { handleFieldChange }/>
            </div>
          </TabPanel>

          <TabPanel value={selectedTab} index={3}>
            <DataGrid style={{ width: "100%" }} autoHeight
                      experimentalFeatures={{ newEditingApi: true }}
                      processRowUpdate={handleRowUpdate}
                      rows={project.externalUrls}
                      page={page} onPageChange={(newPage) => setPage(newPage)}
                      pageSize={5} rowsPerPageOptions={[5]}
                      components={{ Toolbar: CustomToolbar }}
                      columns={[
                        { field: "displayName", flex: 0.6, headerName: t("ExternalUrl.DisplayName"), type: "string", editable: true },
                        { field: "url", flex: 1, headerName: t("ExternalUrl.Url"), type: "string", editable: true },
                        {
                          field: "",
                          headerName: t("Actions.this"),
                          renderCell: (params: GridRenderCellParams<any, ExternalUrl, any>) => {
                            return [
                            <GridActionsCellItem showInMenu={true}
                                  key={`delete-button-${params.row.id}`}
                                  label={t("Actions.Delete")}
                                  onClick={() => { deleteExternalUrl(params.row.id!); }}
                              />];
                          }
                        }
                      ]}/>
          </TabPanel>
        </DialogContent>
      }

        <DialogActions>
          <Button onClick={close}> {t("Actions.Cancel")} </Button>
          <Button onClick={deleteProject}> {t("Actions.Delete")} </Button>
          <Button onClick={saveProject}> {t("Actions.Save")} </Button>
        </DialogActions>
      </Dialog>
  );
};

export { EditProjectDialog };
