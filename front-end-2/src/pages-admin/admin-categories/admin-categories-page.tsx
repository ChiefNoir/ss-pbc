import "./admin-categories-page.scss";
import { useTranslation } from "react-i18next";
import "../../locales/i18n";
import { useEffect, useState } from "react";
import { Category, PublicApi } from "../../services";
import { DataGrid, GridActionsCellItem, GridRenderCellParams, GridToolbarContainer } from "@mui/x-data-grid";
import { Button } from "@mui/material";
import { EditCategoryDialog } from "./features/edit-category";

function AdminCategoriesPage() {
  const { t } = useTranslation();
  const [isLoading, setLoading] = useState(false);
  const [categories, setCategories] = useState(Array<Category>);
  const [alef, setCategory] = useState(new Category());
  const [open, setOpen] = useState(false);

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      const result = await PublicApi.getCategories();

      setCategories(result.data.data);
      setLoading(false);
    };

    fetchData();
  }, []);

  function Merge(category: Category) {
    const ss = categories.filter(x => x.id !== category.id).concat(category);
    setCategories(ss);
  }

  function Remove(category: Category) {
    const ss = categories.filter(x => x.id !== category.id);
    setCategories(ss);
  }

  function GridToolbar() {
    return (
      <GridToolbarContainer>
        <Button onClick={ addCategory } variant="text">
          { t("Admin.Add") }
        </Button>
      </GridToolbarContainer>
    );
  }

  function addCategory() {
    setCategory(new Category());
    setOpen(true);
  }

  function editCategory(id: number) {
    setCategory(categories!.find(x => x.id === id)!);
    setOpen(true);
  }

  return (
<div>
  <EditCategoryDialog category={alef} isOpen ={open} setOpen = {setOpen} merge={Merge} remove = {Remove} />
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
                        label={ t("Admin.Edit") }
                        onClick={() => {
                          editCategory(params.row.id!);
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
