import { FunctionComponent } from "react";
import { useTranslation } from "react-i18next";
import { NavLink } from "react-router-dom";
import { Category } from "../../../../services";
import "./button-category.scss";

const ButtonCategoryComponent: FunctionComponent<{category: Category}> = (props) => {
  const { t } = useTranslation();
  const category = props.category;

  return (
    <NavLink to={`/projects?category=${category.code}`}
             className = "button-flip"
             data-back = {`${t("Category.TotalProjectsShort")}: ${category.totalProjects}` }
             data-front = { category.displayName }>
    </NavLink>
  );
};

export { ButtonCategoryComponent };
