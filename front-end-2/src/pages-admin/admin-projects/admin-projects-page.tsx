import "./admin-projects-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { useEffect, useState } from "react";
import { Incident, Project, ProjectPreview, PublicApi } from "../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { Button } from "@mui/material";
import { Calc, Convert } from "../../helpers";
import { EditProjectDialog } from "./features/edit-project";

function AdminProjectsPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [projects, setProjects] = useState<ProjectPreview[]>([]);
  const [selectedProjectCode, setSelectedProjectCode] = useState<string|null>(null);
  const [incident, setIncident] = useState<Incident|null>(null);
  const [page, setPage] = useState(0);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [rowCountState, setRowCountState] = useState(0);
  const projectPerPage = Convert.ToRestrictedNumber(process.env.REACT_APP_PAGING_PROJECTS_MAX, 1);

  useEffect(() => {
    const fetchData = async() => {
      let pg = page;
      if (pg < 0) {
        pg = 0;
        setPage(0);
      }

      const catResponse = await PublicApi.getCategories();
      if (!catResponse.data.isSucceed) {
        setIncident(catResponse.data.error);
        return;
      }
      const totalPages = Calc.Pages(catResponse.data.data.find(x => x.isEverything)!.totalProjects);
      setRowCountState(totalPages);

      const prjResponse = await PublicApi.getProjects(pg + 1, null);
      if (prjResponse.data.isSucceed) {
        setIncident(catResponse.data.error);
        return;
      }

      setProjects(prjResponse.data.data);
    };

    setLoading(true);
    fetchData();

    setLoading(false);
  }, [page]);

  function GridToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ add } variant="text">
          { t("Actions.Add") }
        </Button>
      </GridToolbarContainer>
    );
  }

  function merge(project: Project) {
    setPage(-1); // TODO: it is just refresh
  }

  function remove(project: Project) {
    setPage(-1); // TODO: it is just refresh
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
    {incident !== null && <div className="edit-category-error"> {incident.message}</div>}
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
              components={{ Toolbar: GridToolbar }}
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
                  headerName: t("Actions.this"),
                  renderCell: (params: GridRenderCellParams<any, ProjectPreview, any>) => {
                    return [
                    <GridActionsCellItem
                        showInMenu = {true}
                        key={`edit-button-${params.row.code}`}
                        label={ t("Actions.Edit") }
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
