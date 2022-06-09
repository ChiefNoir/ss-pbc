import { Button, Tab, Tabs, TextField } from "@mui/material";
import React, { ChangeEvent, FunctionComponent } from "react";
import { useTranslation } from "react-i18next";
import { Introduction } from "../../../../services";
import "./edit-introduction-component.scss";
import { DataGrid, GridToolbarContainer } from "@mui/x-data-grid";

const EditIntroductionComponent: FunctionComponent<{introduction: Introduction}> = (props) => {
  const { t } = useTranslation();

  const intro = props.introduction;
  const [page, setPage] = React.useState(0);
  const [selectedTab, setValue] = React.useState(0);

  interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
  }

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

  function addNewRow() {
    // const ss = new ExternalUrl();
    // ss.id = Math.min(...intro.externalUrls.map(o => o.id!)) - 1;
    // props.setIntroduction((prevState: Introduction) => ({
    //   ...prevState,
    //   externalUrls: intro.externalUrls.concat(ss)
    // }));
  };

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
         <Button onClick={addNewRow} variant="text"> {t("Admin.DoLogin")} </Button>
      </GridToolbarContainer>
    );
  }

  const handleChangeTab = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  function handleSimpleChange(e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    // intro.content = e.target.value;
    // props.update(e.target.value);
  };

  return (
<div className="admin-introduction-container">
  <h1> {t("Introduction.Introduction")} </h1>
  <Tabs value={selectedTab} onChange={handleChangeTab}>
    <Tab label={t("Introduction.Content")} />
    <Tab label={t("Introduction.Poster")} />
    <Tab label={t("Introduction.ExternalUrls.ExternalUrls")} />
  </Tabs>
  <TabPanel value={selectedTab} index={0}>
    <TextField value={intro.content}
               label={t("Introduction.Content")}
               fullWidth multiline rows={15}
               name="content"
               onChange = { handleSimpleChange }
               />
  </TabPanel>
  <TabPanel value={selectedTab} index={1}>
    <img className="admin-introduction-poster" src={ intro.posterPreview || intro.posterUrl } />
    <TextField value={intro.posterDescription}
               label={t("Introduction.PosterDescription")}
               fullWidth/>
  </TabPanel>
  <TabPanel value={selectedTab} index={2}>
    <DataGrid style={{ height: 400, width: "100%", padding: 0 }}
        rows={intro.externalUrls}
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

</div>
  );
};

export { EditIntroductionComponent };
