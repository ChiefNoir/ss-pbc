import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';
import { PagingInfo } from 'src/app/model/PagingInfo';


@Component({
    selector: 'app-projects-list',
    templateUrl: './projects-list.component.html',
    styleUrls: ['./projects-list.component.scss']
  })

export class ProjectsListComponent implements OnInit {

  private service: DataService;
  public projects$: BehaviorSubject<Array<Project>> = new BehaviorSubject<Array<Project>>(null);
  public pagingInfo$: BehaviorSubject<PagingInfo> = new BehaviorSubject<PagingInfo>(null);


  public constructor(service: DataService) {
    this.service = service;
  }


  public ngOnInit(): void {
    this.service.getProjects(0, 200, 'all')
                .subscribe
                (
                  result => this.handleRequestResult(result),
                  error => this.handleError(error)
                );
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
