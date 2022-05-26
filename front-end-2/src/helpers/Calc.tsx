import Convert from "./Convert";

export default class Calc {

    public static Pages(totalItems: number): number {
        var projectPerPage = Convert.ToRestrictedNumber(process.env.REACT_APP_PAGING_PROJECTS_MAX, 1);
        return Math.ceil(totalItems / projectPerPage)
    }
}