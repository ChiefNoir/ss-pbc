import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';
import { Category } from 'src/app/model/Category';
import { PagingInfo } from 'src/app/model/PagingInfo';

@Component({
    selector: 'app-projects-list',
    templateUrl: './projects-list.component.html',
    styleUrls: ['./projects-list.component.scss']
  })

export class ProjectsListComponent {

  private service: DataService;
  private router: Router;
  private activeRoute: ActivatedRoute;
  private projectsPerPage = 2;

  public projects$: BehaviorSubject<Array<Project>> = new BehaviorSubject<Array<Project>>(null);
  public pagingInfo$: BehaviorSubject<PagingInfo> = new BehaviorSubject<PagingInfo>(null);
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);

  public constructor(service: DataService, router: Router, activeRoute: ActivatedRoute) {
    this.service = service;
    this.router = router;
    this.activeRoute = activeRoute;

    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });
  }

  private refreshPage(): void
  {
    this.projects$.next(null);
    this.pagingInfo$.next(null);

    const categoryCode = this.activeRoute.snapshot.paramMap.get('category');
    if (!categoryCode) {
      this.router.navigate(['/projects/all']);
      return;
    }

    if (this.categories$.value == null)
    {
      this.service.getCategories()
                  .then
                  (
                    data =>{this.handleCategories(data)}
                  );
    }

    const currentPage = + this.activeRoute.snapshot.paramMap.get('page');

    this.service.getProjects(currentPage * this.projectsPerPage, this.projectsPerPage, categoryCode)
                  .then
                  (
                    data =>{this.handleProjects(data)}
                  );
  }

  private handleTotalProjects(data: number, categoryCode: string): void
  {
    const pgInfo: PagingInfo =
    {
      minPage: 0,
      maxPage: this.calcMaxPageNumber(data),
      currentPage: 0,
      previousPageUrl: '',
      nextPageUrl: '',
      navigateCallback: this.changePage.bind(this)
    };

    pgInfo.currentPage = +this.activeRoute.snapshot.paramMap.get('page');

    if (pgInfo.currentPage > pgInfo.maxPage) {
        pgInfo.currentPage = pgInfo.maxPage;
      }

    if (pgInfo.currentPage < pgInfo.minPage) {
      pgInfo.currentPage = pgInfo.minPage;
      }

    

    pgInfo.nextPageUrl = this.router.createUrlTree
    (
      ['/projects', categoryCode, pgInfo.currentPage + 1]
    ).toString();

    pgInfo.previousPageUrl = this.router.createUrlTree
    (
      ['/projects', categoryCode, pgInfo.currentPage - 1]
    ).toString();

    this.pagingInfo$.next(pgInfo);
  }

  private handleProjects(data: RequestResult<Array<Project>>): void {
    if (data.isSucceed)
    {
    this.projects$.next(data.data);
    } else{
      this.handleError(data.errorMessage);
    }
  }

  private handleCategories(result: RequestResult<Array<Category>>): void {
    if (result.isSucceed)
    {
      const router = this.router;

      result.data.forEach((value) =>
      {
        value.url = router.createUrlTree(['/projects', value.code]).toString();
      });

      this.categories$.next(result.data.filter(x=>x.totalProjects > 0).sort((a, b) => b.totalProjects - a.totalProjects));

      const categoryCode = this.activeRoute.snapshot.paramMap.get('category');

      const totalProjects = !result.data? 0:  result.data.find(x=>x.code == categoryCode).totalProjects;
      this.handleTotalProjects(totalProjects, categoryCode);
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private changePage(value: number): void {
    if (!this.pagingInfo$){
      return;
    }

    if (value >= this.pagingInfo$.value.maxPage) {
      value = this.pagingInfo$.value.maxPage;
    }

    if (value < this.pagingInfo$.value.minPage) {
      value = this.pagingInfo$.value.minPage;
    }

    const categoryCode = this.activeRoute.snapshot.paramMap.get('category');
    this.router.navigate(['/projects', categoryCode, value]);
  }

  private calcMaxPageNumber(totalProjects: number): number {
    return Math.ceil(totalProjects / this.projectsPerPage) - 1;
  }

  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
