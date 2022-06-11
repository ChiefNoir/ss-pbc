import "./admin-projects-page.scss";
import { useTranslation } from "react-i18next";
import "../../../locales/i18n";
import { useEffect, useState } from "react";
import { ProjectPreview, PublicApi } from "../../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { NavigationAdmin } from "../../../features";
import { Button } from "@mui/material";
import { Calc, Convert } from "../../../helpers";

function AdminProjectsPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [projects, setProjects] = useState(Array<ProjectPreview>);
  const [page, setPage] = useState(0);
  const [rowCountState, setRowCountState] = useState(0);
  const projectPerPage = Convert.ToRestrictedNumber(process.env.REACT_APP_PAGING_PROJECTS_MAX, 1);

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      if (rowCountState === 0) {
        const categoriesResponse = await PublicApi.getCategories();
        const totalCount = Calc.Pages(categoriesResponse.data.data.find(x => x.isEverything)!.totalProjects);
        setRowCountState(totalCount);
      }

      const result = await PublicApi.getProjects(page + 1, null);

      setProjects(result.data.data);
      setLoading(false);
    };

    fetchData();
  }, [page, setRowCountState]);

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
              rows={projects}
              rowCount={rowCountState}
              page={page} pageSize={projectPerPage}
              rowsPerPageOptions = {[projectPerPage]}
              onPageChange={(newPage) => setPage(newPage)}
              loading={isLoading}
              pagination
              paginationMode="server"
              components={{
                Toolbar: GridToolbar
              }}
              columns={[
                { field: "code", headerName: t("Project.Code"), width: 200 },
                { field: "displayName", headerName: t("Project.DisplayName"), flex: 1 },
                {
                  field: "category",
                  headerName: t("Project.Category"),
                  flex: 0.3,
                  valueGetter: (params) => {
                    return params.row.category.displayName;
                  }
                },
                {
                  field: "",
                  width: 70,
                  sortable: false,
                  headerName: t("Admin.Actions"),
                  renderCell: (params: GridRenderCellParams<any, ProjectPreview, any>) => {
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

export { AdminProjectsPage };
