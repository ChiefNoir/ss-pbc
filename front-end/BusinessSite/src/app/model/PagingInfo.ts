import { environment } from 'src/environments/environment';

export class PagingInfo {
  public maxPage: number;
  public minPage: number;
  public currentPage: number;
}

export class Paging {
  private minPage: number = 0;
  private currentPage: number;


  private maxItems: number;
  private maxItemsPerPage: number;

  constructor(currentPage: number, maxItemsPerPage: number, maxItems: number)
  {

    this.maxItems = maxItems;
    this.maxItemsPerPage = maxItemsPerPage;

    if (currentPage > this.getMaxPage()) {
      this.currentPage = this.getMaxPage();
    } else if (currentPage < this.getMinPage()) {
      this.currentPage = this.getMinPage();
    } else {
      this.currentPage = currentPage;
    }
  }

  public getMaxItems(): number {
    return this.maxItems;
  }

  public getMaxPage(): number {
    return Math.ceil(this.maxItems / this.maxItemsPerPage) - 1;
  }

  public getMinPage(): number {
    return this.minPage;
  }

  public getCurrentPage(): number {
    return this.currentPage;
  }
}