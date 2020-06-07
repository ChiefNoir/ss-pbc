import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';
import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-project-list',
    templateUrl: './project.component.html',
    styleUrls: ['./project.component.scss']
  })

export class ProjectComponent {

  private service: DataService;
  private router: Router;
  private activeRoute: ActivatedRoute;
  private titleService: Title;

  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);

  public constructor(service: DataService, router: Router, activeRoute: ActivatedRoute, titleService: Title) {
    this.service = service;
    this.activeRoute = activeRoute;
    this.router = router;
    this.titleService = titleService;

    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });
  }

  private refreshPage(): void
  {
    this.project$.next(null);

    const code = this.activeRoute.snapshot.paramMap.get('code');

    this.service.getProject(code)
                .then
                (
                  data => {this.handleProject(data); }
                );
  }



  private handleProject(data: RequestResult<Project>): void {
    if (data.isSucceed)
    {
      if (data.data == null) {
        this.router.navigate(['/404']);
      }

      this.titleService.setTitle(environment.siteName + ' - ' + data.data?.displayName);
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
