import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditProjectComponent } from 'src/app/component/dialog-edit-project/dialog-edit-project.component';
import { ProjectPreview } from 'src/app/model/ProjectPreview';
import { MessageType, MessageDescription } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';
import { Paging } from 'src/app/model/PagingInfo';

@Component({
  selector: 'app-admin-edit-projects',
  templateUrl: './admin-edit-projects.component.html',
  styleUrls: ['./admin-edit-projects.component.scss'],
})

export class AdminEditProjectsComponent
{
  private service: DataService;
  public columns: string[] = ['code', 'displayName', 'category', 'releaseDate'];

  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);
  public paging$: BehaviorSubject<Paging<string>> = new BehaviorSubject<Paging<string>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });
  public dialog: MatDialog;

  public constructor(service: DataService, dialog: MatDialog)
  {
    this.service = service;
    this.dialog = dialog;

    this.paging$.subscribe(value => this.refreshProjects(value));
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
    this.message$.next({text: StaticNames.LoadInProgress, type: MessageType.Spinner });

    if (!paging)
    {
      this.service.getEverythingCategory()
                  .then
                  (
                    response =>
                    {
                      this.paging$.next(new Paging(0, environment.paging.maxProjects, response.data.totalProjects, response.data.code));
                    },
                    reject => this.handleError(reject)
                  );
    }
    else
    {
      this.service.getProjectsPreview
      (
        paging.getCurrentPage() * environment.paging.maxProjects,
        environment.paging.maxProjects,
        paging.getSearchParam()
      )
      .then
      (
        response => {
          this.message$.next(null);
          this.projects$.next(response.data);
        },
        reject =>  this.handleError(reject)
      );
    }
  }

  private handleError(error: any): void
  {
    this.message$.next({text: error, type: MessageType.Error});
    console.log(error);
  }

}
