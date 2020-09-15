import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditProjectComponent } from 'src/app/component/dialog-edit-project/dialog-edit-project.component';
import { ProjectPreview } from 'src/app/model/ProjectPreview';
import { MessageType, MessageDescription } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';
import { Paging } from 'src/app/model/PagingInfo';
import { Incident, RequestResult } from 'src/app/model/RequestResult';
import { AuthGuard } from 'src/app/guards/authGuard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-edit-projects',
  templateUrl: './admin-edit-projects.component.html',
  styleUrls: ['./admin-edit-projects.component.scss'],
})

export class AdminEditProjectsComponent implements OnInit, OnDestroy
{
  private service: DataService;
  public columns: string[] = ['code', 'displayName', 'category', 'releaseDate'];

  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);
  public paging$: BehaviorSubject<Paging<string>> = new BehaviorSubject<Paging<string>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });
  public dialog: MatDialog;
  private authGuard: AuthGuard;
  private router: Router;

  public constructor(service: DataService, dialog: MatDialog, authGuard: AuthGuard, router: Router)
  {
    this.service = service;
    this.dialog = dialog;
    this.authGuard = authGuard;
    this.router = router;
  }

  public async ngOnInit(): Promise<void>
  {
    await this.authGuard.checkIsLogged();
    if (this.authGuard.isLoggedIn$.value)
    {
      this.paging$.subscribe(value => this.refreshProjects(value));
    }
    else
    {
      this.router.navigate(['/login']);
    }
  }

  public ngOnDestroy(): void
  {
    this.paging$.unsubscribe();
  }

  public showProject(projectCode: any): void
  {
    const dialogRef = this.dialog.open
    (
      DialogEditProjectComponent,
      {width: '90%', minHeight: '80%', data: projectCode}
    );

    dialogRef.afterClosed()
             .subscribe
             (
               (result) =>
               {
                 this.changePage(this.paging$.value.getCurrentPage());
                }
            );
  }

  public skipPage(amount: number): void
  {
    this.changePage(this.paging$.value.getCurrentPage() + amount);
  }

  public changePage(page: number): void
  {
    this.paging$.next(new Paging(page, environment.paging.maxProjects, this.paging$.value.getMaxItems(), this.paging$.value.getSearchParam()));
  }

  private refreshProjects(paging: Paging<string>): void
  {
    this.projects$.next(null);
    this.message$.next({type: MessageType.Spinner });

    if (!paging)
    {
      this.service.getEverythingCategory()
                  .then
                  (
                    response =>
                    {
                      if (response.isSucceed)
                      {
                        this.paging$.next(new Paging(0, environment.paging.maxProjects, response.data.totalProjects, response.data.code));
                      }
                      else
                      {
                        this.handleIncident(response.error);
                      }
                    },
                    reject => this.handleError(reject)
                  );
    }
    else
    {
      this.service.getProjectsPreview
      (
        0,
        environment.paging.maxProjects,
        paging.getSearchParam()
      )
        .then
        (
          response =>
          {
            this.message$.next(null);
            this.handleProjects(response);
          },
          reject =>  this.handleError(reject)
        );
    }
  }

  private handleProjects(result: RequestResult<ProjectPreview[]>): void
  {
    if (result.isSucceed)
    {
      this.projects$.next(result.data);
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
