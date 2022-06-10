import React, { useState, useEffect, ChangeEvent } from "react";
import "./admin-introduction-page.scss";
import { Loader, TabPanel } from "../../ui";
import { PublicApi, Introduction, ExternalUrl, PrivateApi } from "../../services";
import { useTranslation } from "react-i18next";
import { Button, Tab, Tabs, TextField } from "@mui/material";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridRowId, GridToolbarContainer } from "@mui/x-data-grid";

function AdminIntroductionPage() {
  const [introduction, setIntroduction] = useState<Introduction>(new Introduction());
  const [loading, setLoading] = useState(true);
  const [selectedTab, setValueSelectedTab] = React.useState(0);
  const [page, setPage] = React.useState(0);
  const { t } = useTranslation();

  useEffect(() => {
    const fetchIntroduction = async() => {
      setLoading(true);

      const result = await PublicApi.getIntroduction();

      setIntroduction(result.data.data);
      setLoading(false);
    };

    fetchIntroduction();
  }, []);

  async function save() {
    setLoading(true);
    const result = await PrivateApi.saveIntroduction(introduction);

    setIntroduction(result.data.data);
    setLoading(false);
  }

  function addExternalUrl() {
    let minId = Math.min(...introduction.externalUrls.map(o => o.id!)) - 1;
    if (minId === Infinity) {
      minId = -1;
    }

    setIntroduction((prevState: Introduction) => ({
      ...prevState,
      externalUrls: introduction.externalUrls.concat(new ExternalUrl({ id: minId }))
    }));
  };

  function deleteExternalUrl(id: GridRowId) {
    setIntroduction((prevState: Introduction) => ({
      ...prevState,
      externalUrls: introduction.externalUrls.filter(x => x.id !== id)
    }));
  };

  function handleFieldChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setIntroduction((prevState: Introduction) => ({
      ...prevState,
      [e.target.name]: e.target.value
    }));
  };

  function handleChangeTab(_event: React.SyntheticEvent, newValue: number) {
    setValueSelectedTab(newValue);
  };

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ addExternalUrl } variant="text">
          { t("Introduction.ExternalUrls.AddNew")}
        </Button>
      </GridToolbarContainer>
    );
  }

  if (loading) {
    return <Loader />;
  } else {
    return (
<div className="admin-introduction-container">
  <h1> {t("Introduction.Introduction")} </h1>
  <Tabs value={selectedTab} onChange={handleChangeTab}>
    <Tab label={t("Introduction.Content")}/>
    <Tab label={t("Introduction.Poster")}/>
    <Tab label={t("Introduction.ExternalUrls.ExternalUrls")}/>
  </Tabs>

  <TabPanel value={selectedTab} index={0}>
    <div className="admin-introduction-content">
    <TextField name="content" fullWidth multiline rows= {15}
               label={t("Introduction.Content")}
               value={introduction.content}
               onChange={handleFieldChange}
               />
            </div>
  </TabPanel>

  <TabPanel value={selectedTab} index={1}>
    <div className="admin-introduction-content">
      <img className="admin-introduction-poster"
           src={introduction.posterPreview || introduction.posterUrl}/>
      <TextField value={introduction.posterUrl}
                 label={t("Introduction.PosterUrl")}
                 fullWidth/>
      <TextField value={introduction.posterDescription}
                 label={t("Introduction.PosterDescription")}
                 fullWidth/>
    </div>
  </TabPanel>

  <TabPanel value={selectedTab} index={2}>
    <DataGrid style={{ height: 420, width: "100%", padding: 0 }}
        rows={introduction.externalUrls}
        page={page}
        onPageChange={(newPage) => setPage(newPage)}
        pageSize={10}
        rowsPerPageOptions={[10]}
        components={{
          Toolbar: CustomToolbar
        }}
        columns={[
          { field: "id", headerName: t("Introduction.ExternalUrls.Id"), editable: false },
          { field: "displayName", flex: 0.6, headerName: t("Introduction.ExternalUrls.DisplayName"), type: "string", editable: true },
          { field: "url", flex: 1, headerName: t("Introduction.ExternalUrls.Url"), type: "string", editable: true },
          { field: "version", headerName: t("Introduction.ExternalUrls.Version"), type: "string", editable: false },
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
                  color="inherit"
                />
              ];
            }
          }
        ]}

      />
  </TabPanel>

  <Button onClick={save} variant="outlined"> {t("Admin.Save")} </Button>
</div>);
  }
}

export { AdminIntroductionPage };
