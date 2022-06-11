import "./admin-categories-page.scss";
import { useTranslation } from "react-i18next";
import "../../../locales/i18n";
import { useEffect, useState } from "react";
import { Category, PublicApi } from "../../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { NavigationAdmin } from "../../../features";
import { Button } from "@mui/material";

function AdminCategoriesPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [categories, setCategories] = useState(Array<Category>);

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      const result = await PublicApi.getCategories();

      setCategories(result.data.data);
      setLoading(false);
    };

    fetchData();
  }, []);

  function GridToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ addProject } variant="text">
          { t("Project.Actions.Add") }
        </Button>
      </GridToolbarContainer>
    );
  }

  function addProject() {
    console.log("addProject");
  }

  function editProject(code: string) {
    console.log(`editProject: ${code}`);
  }

  return (
<div>
  <NavigationAdmin />
  <div style={{ display: "flex", height: "100%" }}>
    <DataGrid style={{ width: "100%" }} autoHeight
              getRowId={(row) => row.code}
              rows={categories}
              rowCount={categories.length}
              page={0} pageSize={100}
              rowsPerPageOptions = {[100]}
              loading={isLoading}
              pagination
              paginationMode="server"
              components={{
                Toolbar: GridToolbar
              }}
              columns={[
                { field: "id", headerName: t("Category.Id"), width: 200 },
                { field: "code", headerName: t("Category.Code"), width: 200 },
                { field: "displayName", headerName: t("Category.DisplayName"), flex: 1 },
                { field: "isEverything", headerName: t("Category.IsEverything"), width: 200 },
                { field: "totalProjects", headerName: t("Category.TotalProjects"), width: 200 },
                {
                  field: "",
                  width: 70,
                  sortable: false,
                  headerName: t("Admin.Actions"),
                  renderCell: (params: GridRenderCellParams<any, Category, any>) => {
                    return [
                    <GridActionsCellItem
                        showInMenu = {true}
                        key={`edit-button-${params.row.code}`}
                        label={ t("Project.Actions.Edit") }
                        onClick={() => {
                          editProject(params.row.code);
                        }}/>
                    ];
                  }
                }
              ]}
    />
      </div>

</div>
  );
}

export { AdminCategoriesPage };
