import { Convert } from "./Convert";

class Calc {
  public static Pages(totalItems: number): number {
    const maxProjects = window._env_.REACT_APP_PAGING_PROJECTS_MAX;

    const projectPerPage = Convert.ToRestrictedNumber(maxProjects, 1);
    return Math.ceil(totalItems / projectPerPage);
  }
}

export { Calc };
