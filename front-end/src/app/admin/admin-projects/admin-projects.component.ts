import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { PublicService } from '../../core/services/public.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditProjectComponent } from '../dialog-edit-project/dialog-edit-project.component';
import { ProjectPreview } from '../../projects/project-preview.model';
import { MessageType, MessageDescription } from '../../shared/message/message.component';
import { ResourcesService } from '../../core/services/resources.service';
import { Paging } from '../../shared/paging-info.model';
import { RequestResult } from '../../shared/request-result.interface';
import { Incident } from '../../shared/incident.interface'
import { Category } from '../../shared/category.model';

@Component({
  selector: 'app-admin-projects',
  templateUrl: './admin-projects.component.html',
  styleUrls: ['./admin-projects.component.scss'],
})
export class AdminProjectsComponent implements OnInit, OnDestroy {
  public columns: string[] = ['code', 'displayName', 'category', 'releaseDate'];
  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);
  public paging$: BehaviorSubject<Paging<string>> = new BehaviorSubject<Paging<string>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });

  public constructor(
    private service: PublicService,
    public dialog: MatDialog,
    public textMessages: ResourcesService
  ) {}

  public ngOnInit(): void {
    this.paging$.subscribe((value) => this.refreshProjects(value));
  }

  public ngOnDestroy(): void {
    this.paging$.unsubscribe();
  }

  private handleCategory(result: RequestResult<Category>): void {
    if (result.isSucceed) {
      this.paging$.next(
        new Paging(
          0,
          environment.paging.maxProjects,
          result.data.totalProjects,
          result.data.code
        )
      );
    } else {
      this.handleError(result.error);
    }
  }

  private refreshProjects(paging: Paging<string>): void {
    this.projects$.next(null);
    this.message$.next({ type: MessageType.Spinner });

    if (!paging) {
      this.service.getEverythingCategory().subscribe(
        win => this.handleCategory(win),
        fail => this.handleError(fail)
      );

      return;
    }

    this.service
      .getProjectsPreview(
        paging.getCurrentPage() * environment.paging.maxProjects,
        environment.paging.maxProjects,
        paging.getSearchParam()
      )
      .subscribe(
        (win) => this.handleProjects(win),
        (fail) => this.handleError(fail)
      );
  }

  public showDialog(projectCode?: string): void {
    const dialogRef = this.dialog.open(DialogEditProjectComponent, {
      width: '90%',
      minHeight: '90%',
      data: projectCode,
    });

    dialogRef
      .afterClosed()
      .toPromise()
      .then(() => this.changePage(this.paging$.value.getCurrentPage()));
  }

  public skipPage(amount: number): void {
    this.changePage(this.paging$.value.getCurrentPage() + amount);
  }

  public changePage(page: number): void {
    this.paging$.next(
      new Paging(
        page,
        environment.paging.maxProjects,
        this.paging$.value.getMaxItems(),
        this.paging$.value.getSearchParam()
      )
    );
  }

  private handleProjects(result: RequestResult<ProjectPreview[]>): void {
    if (result.isSucceed) {
      this.message$.next(null);
      this.projects$.next(result.data);
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
