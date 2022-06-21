import "./showcase-page.scss";
import { ButtonCategoryComponent, ProjectPreviewComponent } from "./features/";
import { Convert, Calc } from "../../helpers";
import { Link, useSearchParams } from "react-router-dom";
import { ErrorComponent, Loader } from "../../ui";
import { Pagination, PaginationItem } from "@mui/material";
import { PublicApi, Category, ProjectPreview, Incident } from "../../services";
import { useEffect, useState } from "react";

function ShowcasePage() {
  const [searchParams] = useSearchParams();
  const categoryCode = searchParams.get("category");
  const pageNumber = Convert.ToRestrictedNumber(searchParams.get("page"), 1);

  const [projects, setProjects] = useState<ProjectPreview[] | null>(null);
  const [categories, setCategories] = useState<Category[] | null>(null);
  const [selectedCategory, setSelectedCategory] = useState<Category>();
  const [incident, setIncident] = useState<Incident|null>(null);

  useEffect(() => {
    const fetchData = async() => {
      setIncident(null);

      let tmpCategories = categories ?? null;

      if (tmpCategories === null) {
        const categoriesResponse = await PublicApi.getCategories();

        if (!categoriesResponse.data.isSucceed) {
          setIncident(categoriesResponse.data.error);
          return;
        }

        tmpCategories = categoriesResponse.data.data;
      }

      if (categoryCode === null) {
        setSelectedCategory(tmpCategories.find(x => x.isEverything));
      } else {
        setSelectedCategory(tmpCategories.find(x => x.code === categoryCode));
      }

      const projectsResponse = await PublicApi.getProjects(pageNumber, categoryCode);
      if (!projectsResponse.data.isSucceed) {
        setIncident(projectsResponse.data.error);
        return;
      }

      if (categories === null) {
        setCategories(tmpCategories);
      }
      setProjects(projectsResponse.data.data);
    };

    fetchData();
  }, [searchParams]);

  if (incident) {
    return <ErrorComponent message={incident.message} detail={incident.detail}/>;
  }

  return (
    <div className="showcase-container">
      <div className="showcase-filter">
      {categories != null
        && (categories?.filter(x => x.totalProjects > 0).map(
          x => {
            return <ButtonCategoryComponent key={`category-btn-${x.id}`} category={x} />;
          }
        ))
      }
      </div>
      <div className="showcase-projects">
      {projects != null
        ? projects?.map(
          x => {
            return <ProjectPreviewComponent key={`project-cared-${x.code}`} project={x} />;
          }
        )
        : (<Loader />)
      }
      </div>

      {Calc.Pages(selectedCategory?.totalProjects ?? 0) > 1
      && <div className="showcase-paging">
          <Pagination page={pageNumber}
                      count={Calc.Pages(selectedCategory?.totalProjects ?? 0)}
                      size = "large"
                      shape="rounded"
                      renderItem={(item) => (
                          <PaginationItem component={Link}
                                          to={`/projects/?category=${selectedCategory?.code}&page=${item.page}`}
                                          {...item} />)}
              />
        </div>
      }
    </div>
  );
}

export { ShowcasePage };
