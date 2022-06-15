import "./admin-categories-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { useEffect, useState } from "react";
import { Category, Incident, PublicApi } from "../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { Button } from "@mui/material";
import { EditCategoryDialog } from "./features/edit-category";
import { ErrorComponent } from "../../ui";

function AdminCategoriesPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [categories, setCategories] = useState(Array<Category>);
  const [category, setCategory] = useState<Category|null>(null);
  const [open, setOpen] = useState(false);
  const [incident, setIncident] = useState<Incident|null>(null);

  useEffect(() => {
    const fetchData = async() => {
      const result = await PublicApi.getCategories();
      if (result.data.isSucceed) {
        setCategories(result.data.data);
      } else {
        setIncident(result.data.error);
      }
    };

    setLoading(true);
    setIncident(null);
    setCategory(null);

    fetchData();

    setLoading(false);
  }, []);

  function merge(category: Category) {
    setCategories(
      categories.filter(x => x.id !== category.id).concat(category)
    );
  }

  function remove(category: Category) {
    setCategories(
      categories.filter(x => x.id !== category.id)
    );
  }

  function edit(id: number) {
    setCategory(categories!.find(x => x.id === id)!);
    setOpen(true);
  }

  function add() {
    setCategory(new Category());
    setOpen(true);
  }

  function GridToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ add } variant="text">
          { t("Actions.Add") }
        </Button>
      </GridToolbarContainer>
    );
  }

  if (incident) {
    return <ErrorComponent message={incident.message} detail={incident.detail}/>;
  }

  return (
  <div>
    <EditCategoryDialog category={category}
                        isOpen={open} setOpen={setOpen}
                        merge={merge} remove = {remove} />

    <DataGrid style={{ width: "100%" }} autoHeight
              getRowId={(row) => row.code}
              rows={categories} rowCount={categories.length}
              page={0} pageSize={100} rowsPerPageOptions = {[100]}
              loading={isLoading}
              components={{ Toolbar: GridToolbar }}
              columns={[
                { field: "code", headerName: t("Category.Code"), width: 200 },
                { field: "displayName", headerName: t("Category.DisplayName"), flex: 1 },
                { field: "isEverything", headerName: t("Category.IsEverything"), width: 200 },
                { field: "totalProjects", headerName: t("Category.TotalProjects"), width: 200 },
                {
                  field: "",
                  width: 70,
                  sortable: false,
                  headerName: t("Actions.this"),
                  renderCell: (params: GridRenderCellParams<any, Category, any>) => {
                    return [
                    <GridActionsCellItem
                        showInMenu = {true}
                        key={`edit-category-button-${params.row.code}`}
                        label={ t("Actions.Edit") }
                        onClick={() => { edit(params.row.id!); }}/>
                    ];
                  }
                }
              ]}/>
      </div>
  );
}

export default AdminCategoriesPage;
