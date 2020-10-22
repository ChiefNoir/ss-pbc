import { BehaviorSubject } from 'rxjs';
import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { environment } from 'src/environments/environment';

import { ResourcesService } from '../../core/resources.service';
import { PublicService } from '../../core/public.service';
import { MessageType, MessageDescription } from '../../shared/message/message.component';
import { Project } from '../../shared/project.model';
import { RequestResult, Incident } from '../../shared/request-result.model';

@Component({
  selector: 'app-project-list',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss'],
})
export class ProjectComponent {
  public message$: BehaviorSubject<MessageDescription>;
  public project$: BehaviorSubject<Project>;

  public constructor(
    public textMessages: ResourcesService,
    private service: PublicService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private titleService: Title
  ) {
    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });

    this.message$ = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });
    this.project$ = new BehaviorSubject<Project>(null);
  }

  private refreshPage(): void {
    this.project$.next(null);

    const code = this.activeRoute.snapshot.paramMap.get('code');
    this.service.getProject(code).then(
      (win) => this.handleProject(win),
      (fail) => this.handleError(fail)
    );
  }

  private handleProject(result: RequestResult<Project>): void {
    if (result.isSucceed) {
      if (result.data == null) {
        this.router.navigate(['/404']);
      }

      this.titleService.setTitle(
        result.data?.displayName +
          this.textMessages.TitleSeparator +
          environment.siteName
      );
      this.project$.next(result.data);
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    console.log(error);
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    console.log(error);

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
