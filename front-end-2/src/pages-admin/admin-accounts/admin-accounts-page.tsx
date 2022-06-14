import "./admin-accounts-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { PrivateApi } from "../../services/PrivateApi";
import { useEffect, useState } from "react";
import { Account, Incident } from "../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { Button } from "@mui/material";
import { EditAccountDialog } from "./features/edit-account";
import { ErrorComponent } from "../../ui";

function AdminAccountsPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [accounts, setAccounts] = useState<Account[]>([]);
  const [selectedAccount, setSelectedAccount] = useState<Account | null>(null);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [incident, setIncident] = useState<Incident|null>(null);

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      const result = await PrivateApi.getAccounts();
      if (result.data.isSucceed) {
        setAccounts(result.data.data);
      } else {
        setIncident(result.data.error);
      }

      setLoading(false);
    };

    setIncident(null);
    fetchData();
  }, []);

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={addAccount} variant="text">
          { t("Actions.Add")}
        </Button>
      </GridToolbarContainer>
    );
  }

  function merge(account: Account) {
    const tmp = accounts.filter(x => x.id !== account.id).concat(account);
    setAccounts(tmp);
  }

  function remove(account: Account) {
    const tmp = accounts.filter(x => x.id !== account.id);
    setAccounts(tmp);
  }

  function addAccount() {
    setSelectedAccount(null);
    setIsDialogOpen(true);
  }

  function editAccount(id: number) {
    setSelectedAccount(accounts!.find(x => x.id === id)!);
    setIsDialogOpen(true);
  }

  if (incident) {
    return <ErrorComponent message={incident.message} detail={incident.detail}/>;
  }

  return (
    <div>
      <EditAccountDialog account={selectedAccount} isOpen ={isDialogOpen}
                         setOpen={setIsDialogOpen} mergeAccount={merge} removeAccount={remove} />

      <DataGrid style={{ width: "100%" }} autoHeight
                rows={accounts} rowCount={accounts.length}
                page={0} pageSize={100} rowsPerPageOptions = {[100]}
                loading={isLoading}
                components={{
                  Toolbar: CustomToolbar
                }}
                columns={[
                  { field: "login", headerName: t("Account.Login"), flex: 0.5 },
                  { field: "role", headerName: t("Account.Role"), flex: 0.5 },
                  {
                    field: "",
                    width: 70,
                    sortable: false,
                    headerName: t("Actions.this"),
                    renderCell: (params: GridRenderCellParams<any, Account, any>) => {
                      return [
                      <GridActionsCellItem
                            showInMenu = {true}
                            key={`delete-button-${params.row.id}`}
                            label={ t("Actions.Edit") }
                            onClick={() => { editAccount(params.row.id!); }}
                            color="inherit"/>
                      ];
                    }
                  }
                ]}/>
    </div>
  );
}

export { AdminAccountsPage };
