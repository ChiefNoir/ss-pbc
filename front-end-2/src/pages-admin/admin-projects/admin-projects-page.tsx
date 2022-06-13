import "./admin-projects-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { useEffect, useState } from "react";
import { Project, ProjectPreview, PublicApi } from "../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { NavigationAdmin } from "../../features";
import { Button } from "@mui/material";
import { Calc, Convert } from "../../helpers";
import { EditProjectDialog } from "./features/edit-project";

function AdminProjectsPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [projects, setProjects] = useState(Array<ProjectPreview>);
  const [selectedProjectCode, setSelectedProjectCode] = useState<string | null>(null);
  const [page, setPage] = useState(0);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
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
        <Button onClick={ add } variant="text">
          { t("Admin.Add") }
        </Button>
      </GridToolbarContainer>
    );
  }

  function merge(project: Project) {
    // const tmp = accounts.filter(x => x.id !== account.id).concat(account);
    // setAccounts(tmp);
  }

  function remove(project: Project) {
    // const tmp = accounts.filter(x => x.id !== account.id);
    // setAccounts(tmp);
  }

  function add() {
    setSelectedProjectCode(null);
    setIsDialogOpen(true);
  }

  function edit(code: string) {
    setSelectedProjectCode(code);
    setIsDialogOpen(true);
  }

  return (
<div>
  <EditProjectDialog projectCode={selectedProjectCode} isOpen ={isDialogOpen}
                         setOpen={setIsDialogOpen} merge={merge} remove={remove} />
    <DataGrid style={{ width: "100%" }} autoHeight
              getRowId={(row) => row.code}
              rows={projects}
              rowCount={projects.length}
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
                        label={ t("Admin.Edit") }
                        onClick={() => {
                          edit(params.row.code);
                        }}/>
                    ];
                  }
                }
              ]}
    />

</div>
  );
}

export { AdminProjectsPage };
