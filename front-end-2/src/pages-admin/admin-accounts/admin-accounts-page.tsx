import "./admin-accounts-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { PrivateApi } from "../../services/PrivateApi";
import { useEffect, useState } from "react";
import { Loader } from "../../ui";
import { Account } from "../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { Button } from "@mui/material";
import { NavigationAdmin } from "../../features";

function AdminAccountsPage() {
  const { t } = useTranslation();
  const [loading, setLoading] = useState(false);
  const [accounts, setAccounts] = useState(Array<Account>);

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ addAccount } variant="text">
          { t("Introduction.ExternalUrls.AddNew")}
        </Button>
      </GridToolbarContainer>
    );
  }

  function addAccount() {
    console.log("");
  }

  function deleteAccount(id: number) {
    console.log("");
  }

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      const result = await PrivateApi.getAccounts();

      setAccounts(result.data.data);
      setLoading(false);
    };

    fetchData();
  }, []);

  if (loading) {
    return <Loader />;
  }

  return (
    <div>
      <NavigationAdmin />
    <DataGrid style={{ height: 420, width: "100%", padding: 0 }}
    rows={accounts}
    page={0}
    pageSize={20}
    rowsPerPageOptions={[20]}
    components={{
      Toolbar: CustomToolbar
    }}
    columns={[
      { field: "login", headerName: t("Account.Login"), editable: true },
      { field: "role", headerName: t("Account.Role"), editable: true },
      {
        field: "",
        headerName: t("Admin.Actions"),
        width: 100,
        renderCell: (params: GridRenderCellParams<any, Account, any>) => {
          return [
            <GridActionsCellItem
            showInMenu = {true}
            key={`delete-button-${params.row.id}`}
              label="Delete"
              onClick={() => {
                deleteAccount(params.row.id!);
              }}
              color="inherit"
            />
          ];
        }
      }
    ]}

  />
  </div>
  );
}

export { AdminAccountsPage };
