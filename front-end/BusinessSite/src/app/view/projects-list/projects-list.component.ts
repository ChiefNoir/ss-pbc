import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';


@Component({
    selector: 'app-projects-list',
    templateUrl: './projects-list.component.html',
    styleUrls: ['./projects-list.component.scss']
  })

export class ProjectsListComponent implements OnInit {

  private service: DataService;
  public projects$: BehaviorSubject<Array<Project>> = new BehaviorSubject<Array<Project>>(null);

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
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void {

    // TODO: react properly
    console.log(error);
  }
}
