import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';

@Component({
  selector: 'app-project-list',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss'],
})

export class ProjectComponent {
  private service: DataService;
  private activeRoute: ActivatedRoute;
  private router: Router;
  private titleService: Title;

  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);

  public constructor(service: DataService, activeRoute: ActivatedRoute, router: Router, titleService: Title) {
    this.service = service;
    this.activeRoute = activeRoute;
    this.router = router;
    this.titleService = titleService;

    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });
  }

  private refreshPage(): void {
    this.project$.next(null);

    const code = this.activeRoute.snapshot.paramMap.get('code');
    this.service.getProject(code)
                .then
                (
                  (data) => { this.handleProjectRequest(data); }
                );
  }

  private handleProjectRequest(result: RequestResult<Project>): void {
    if (result.isSucceed) {
      if (result.data == null) {
        this.router.navigate(['/404']);
      }

      this.titleService.setTitle(environment.siteName + ' - ' + result.data?.displayName);
      this.project$.next(result.data);
    } else {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
