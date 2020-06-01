export class PagingInfo {
    public maxPage: number;
    public minPage: number;
    public currentPage: number;

    public previousPageUrl: string;
    public nextPageUrl: string;

    public navigateCallback: void;
}
