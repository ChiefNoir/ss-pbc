import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';

import { DataService } from 'src/app/service/data.service';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';
import { StaticNames } from 'src/app/common/StaticNames';
import { MessageType, MessageDescription } from 'src/app/component/message/message.component';

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

  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({type: MessageType.Spinner });
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
                  result => this.handleProjectRequest(result),
                  reject => this.handleError(reject)
                );
  }

  private handleProjectRequest(result: RequestResult<Project>): void
  {
    if (result.isSucceed)
    {
      if (result.data == null)
      {
        this.router.navigate(['/404']);
      }

      this.titleService.setTitle(environment.siteName + ' - ' + result.data?.displayName);
      this.project$.next(result.data);
    }
    else
    {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void
  {
    this.message$.next({text: error.code + ' : ' + error.message + '<br/>' + error.detail + '<br/>' , type: MessageType.Error });
  }

  private handleError(error: any): void
  {
    console.log(error);

    if (error.name !== undefined)
    {
      this.message$.next({text: error.name, type: MessageType.Error });
    }
    else
    {
      this.message$.next({text: error, type: MessageType.Error });
    }
  }

}
