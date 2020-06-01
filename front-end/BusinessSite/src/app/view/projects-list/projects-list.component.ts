import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

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

export class ProjectsListComponent implements OnInit {

  private service: DataService;
  private router: Router;

  public projects$: BehaviorSubject<Array<Project>> = new BehaviorSubject<Array<Project>>(null);
  public pagingInfo$: BehaviorSubject<PagingInfo> = new BehaviorSubject<PagingInfo>(null);
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);

  public constructor(service: DataService, router: Router) {
    this.service = service;
    this.router = router;
  }


  public ngOnInit(): void {
    this.service.getProjects(0, 200, 'all')
                .subscribe
                (
                  result => this.handleRequestResult(result),
                  error => this.handleError(error)
                );

    this.service.getCategories()
                .subscribe
                (
                  result => this.handleCategories(result),
                  error => this.handleError(error)
                );
  }

  private handleCategories(result: RequestResult<Array<Category>>): void {

    if (result.isSucceed)
    {
    const router = this.router;
    result.data.forEach((value) => {
      value.url = router.createUrlTree(['/projects', value.code]).toString();
    });

    this.categories$.next(result.data);
  }
  else
  {
    this.handleError(result.errorMessage);
  }
  }

  private handleRequestResult(result: RequestResult<Array<Project>>): void {

    if (result.isSucceed)
    {
      this.projects$.next(result.data);

      const pgInfo: PagingInfo = {
        minPage: 0,
        maxPage: this.calcMaxPageNumber(10),
        currentPage: 0,
  
        previousPageUrl: '',
        nextPageUrl: '',
  
        navigateCallback: this.changePage.bind(this)
      };

      this.pagingInfo$.next(pgInfo);

    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private changePage(value: number): void {}

  private calcMaxPageNumber(totalProjects: number): number {
    return 101;
  }

  private handleError(error: any): void {

    // TODO: react properly
    console.log(error);
  }
}
