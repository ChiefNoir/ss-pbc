export class Paging<T>
{
  private currentPage: number;
  private maxItems: number;
  private maxItemsPerPage: number;
  private minPage: number;
  private searchParam: T;

  constructor(currentPage: number, maxItemsPerPage: number, maxItems: number, searchParam: T = null)
  {
    this.minPage = 0;
    this.maxItems = maxItems;
    this.maxItemsPerPage = maxItemsPerPage;
    this.searchParam = searchParam;

    if (currentPage > this.getMaxPage())
    {
      this.currentPage = this.getMaxPage();
    }
    else if (currentPage < this.getMinPage())
    {
      this.currentPage = this.getMinPage();
    }
    else
    {
      this.currentPage = currentPage;
    }
  }

  public getSearchParam(): T
  {
    return this.searchParam;
  }

  public getMaxItems(): number
  {
    return this.maxItems;
  }

  public getMaxPage(): number
  {
    const val = Math.ceil(this.maxItems / this.maxItemsPerPage) - 1;
    return val < 0 ? 0 : val;
  }

  public getMinPage(): number
  {
    if (this.minPage < 0) { return 0; }

    return this.minPage;
  }

  public getCurrentPage(): number
  {
    return this.currentPage;
  }
}
