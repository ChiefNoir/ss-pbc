import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';

@Component({
    selector: 'app-project-list',
    templateUrl: './project.component.html',
    styleUrls: ['./project.component.scss']
  })

export class ProjectComponent {

  private service: DataService;
  private activeRoute: ActivatedRoute;

  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);

  public constructor(service: DataService, router: Router, activeRoute: ActivatedRoute) {
    this.service = service;
    this.activeRoute = activeRoute;

    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });
  }

  private refreshPage(): void
  {
    this.project$.next(null);

    const code = this.activeRoute.snapshot.paramMap.get('code');

    this.service.getProject(code)
                .subscribe
                (
                  result => this.handleProject(result),
                  error => this.handleError(error)
                );
  }



  private handleProject(data: RequestResult<Project>): void {
    if (data.isSucceed)
    {
      this.project$.next(data.data);
    }
    else
    {
      this.handleError(data.errorMessage);
    }
  }

  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
