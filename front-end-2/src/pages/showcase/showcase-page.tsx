import "./showcase-page.scss";
import { ButtonCategoryComponent, ProjectPreviewComponent } from "./features/";
import { Convert, Calc } from "../../helpers";
import { Link, useSearchParams } from "react-router-dom";
import { Loader } from "../../ui";
import { Pagination, PaginationItem } from "@mui/material";
import { PublicApi, Category, ProjectPreview } from "../../services";
import { useEffect, useState } from "react";

function ShowcasePage() {
  const [searchParams] = useSearchParams();
  const categoryCode = searchParams.get("category");
  const page = searchParams.get("page") ?? "0";

  const [loading, setLoading] = useState(true);
  const [projects, setProjects] = useState<Array<ProjectPreview>>();
  const [categories, setCategories] = useState<Array<Category>>();
  const [selectedCategory, setCategory] = useState<Category>();

  useEffect(() => {
    const fetchData = async() => {
      setLoading(true);

      let tmpCategories = categories ?? [];

      if (categories === undefined) {
        const categoriesResponse = await PublicApi.getCategories();
        tmpCategories = categoriesResponse.data.data;
        setCategories(tmpCategories);
      }

      if (categoryCode === null) {
        setCategory(tmpCategories.find(x => x.isEverything));
      } else {
        setCategory(tmpCategories.find(x => x.code === categoryCode));
      }

      const projectsResponse = await PublicApi.getProjects(Convert.ToRestrictedNumber(page, 1), categoryCode);

      setProjects(projectsResponse.data.data);
      setLoading(false);
    };

    fetchData();
  }, [searchParams]);

  if (loading) {
    return <Loader />;
  } else {
    return (
      <div>
        <div className="projects-categories-filter">
        {
          categories?.filter(x => x.totalProjects > 0).map(
            x => {
              return <ButtonCategoryComponent key={x.id} category={x} />;
            }
          )
        }
        </div>
        <div className="showcase-projects">
        {
          projects?.map(
            x => {
              return <ProjectPreviewComponent key={x.code} project={x} />;
            }
          )
        }
        </div>

        {Calc.Pages(selectedCategory?.totalProjects ?? 0) > 1
        && <div className="projects-paging">
            <Pagination page={ Convert.ToRestrictedNumber(page, 1)}
                        count={ Calc.Pages(selectedCategory?.totalProjects ?? 0)}
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
  };
}

export { ShowcasePage };
