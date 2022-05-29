import Convert from "./Convert";

export class Calc {
    public static Pages(totalItems: number): number {
        const maxProjects = process.env.REACT_APP_PAGING_PROJECTS_MAX;

        var projectPerPage = Convert.ToRestrictedNumber(maxProjects, 1);
        return Math.ceil(totalItems / projectPerPage)
    }
}
