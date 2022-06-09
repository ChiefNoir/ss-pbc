import React, { useState, useEffect, ChangeEvent } from "react";
import "./admin-introduction-page.scss";
import { Loader } from "../../ui";
import { PublicApi, Introduction, ExternalUrl } from "../../services";
import { useTranslation } from "react-i18next";
import { Button, Tab, Tabs, TextField } from "@mui/material";
import { DataGrid, GridToolbarContainer } from "@mui/x-data-grid";

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function AdminIntroductionPage() {
  const [introduction, setIntroduction] = useState<Introduction>(new Introduction());
  const [loading, setLoading] = useState(true);
  const [selectedTab, setValueSelectedTab] = React.useState(0);
  const [page, setPage] = React.useState(0);
  const { t } = useTranslation();

  function addNewRow() {
    const ss = new ExternalUrl();
    ss.id = Math.min(...introduction.externalUrls.map(o => o.id!)) - 1;

    setIntroduction((prevState: Introduction) => ({
      ...prevState,
      externalUrls: introduction.externalUrls.concat(ss)
    }));
  };

  function handleFieldChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    setIntroduction((prevState: Introduction) => ({
      ...prevState,
      [e.target.name]: e.target.value
    }));
  };

  function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;

    return (
      <div
        role="tabpanel"
        hidden={value !== index}
        id={`simple-tabpanel-${index}`}
        aria-labelledby={`simple-tab-${index}`}
        {...other}
      >
        {value === index && (
          <div>
            {children}
          </div>
        )}
      </div>
    );
  }

  const handleChangeTab = (event: React.SyntheticEvent, newValue: number) => {
    setValueSelectedTab(newValue);
  };

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
         <Button onClick={ addNewRow } variant="text"> { t("Admin.DoLogin")} </Button>
      </GridToolbarContainer>
    );
  }

  useEffect(() => {
    const fetchIntroduction = async() => {
      setLoading(true);

      const result = await PublicApi.getIntroduction();

      setIntroduction(result.data.data);
      setLoading(false);
    };

    fetchIntroduction();
  }, []);

  if (loading) {
    return <Loader />;
  } else {
    return (
<div className="admin-introduction-content">
  <h1> {t("Introduction.Introduction")} </h1>
  <Tabs value={selectedTab} onChange={handleChangeTab}>
    <Tab label={t("Introduction.Content")} />
    <Tab label={t("Introduction.Poster")} />
    <Tab label={t("Introduction.ExternalUrls.ExternalUrls")} />
  </Tabs>
  <TabPanel value={selectedTab} index={0}>
    <TextField value={introduction!.content}
               label={t("Introduction.Content")}
               fullWidth multiline rows={15}
               name="content"
               onChange = { handleFieldChange }
               />
  </TabPanel>
  <TabPanel value={selectedTab} index={1}>
    <img className="admin-introduction-poster" src={ introduction!.posterPreview || introduction!.posterUrl } />
    <TextField value={introduction!.posterDescription}
               label={t("Introduction.PosterDescription")}
               fullWidth/>
  </TabPanel>
  <TabPanel value={selectedTab} index={2}>
    <DataGrid style={{ height: 400, width: "100%", padding: 0 }}
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
          { field: "version", headerName: t("Introduction.ExternalUrls.Version"), type: "string", editable: true }]}
        experimentalFeatures={{ newEditingApi: true }}
      />
  </TabPanel>
</div>);
  }
}

export { AdminIntroductionPage };
