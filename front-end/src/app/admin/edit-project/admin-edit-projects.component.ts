import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { PublicService } from 'src/app/core/public.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditProjectComponent } from 'src/app/admin/dialog-edit-project/dialog-edit-project.component';
import { ProjectPreview } from 'src/app/projects/project-preview.model';
import {
  MessageType,
  MessageDescription,
} from 'src/app/shared/message/message.component';
import { ResourcesService } from 'src/app/core/resources.service';
import { Paging } from 'src/app/shared/paging-info.model';
import { Incident, RequestResult } from 'src/app/shared/request-result.model';
import { AuthGuard } from 'src/app/core/auth.guard';
import { Router } from '@angular/router';
import { Category } from 'src/app/shared/category.model';

@Component({
  selector: 'app-admin-edit-projects',
  templateUrl: './admin-edit-projects.component.html',
  styleUrls: ['./admin-edit-projects.component.scss'],
})
export class AdminEditProjectsComponent implements OnInit, OnDestroy {
  private service: PublicService;
  private authGuard: AuthGuard;
  private router: Router;

  public columns: string[] = ['code', 'displayName', 'category', 'releaseDate'];
  public dialog: MatDialog;
  public projects$: BehaviorSubject<
    Array<ProjectPreview>
  > = new BehaviorSubject<Array<ProjectPreview>>(null);
  public paging$: BehaviorSubject<Paging<string>> = new BehaviorSubject<
    Paging<string>
  >(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<
    MessageDescription
  >({ type: MessageType.Spinner });

  public constructor(
    service: PublicService,
    dialog: MatDialog,
    authGuard: AuthGuard,
    router: Router,
    public textMessages: ResourcesService
  ) {
    this.service = service;
    this.dialog = dialog;
    this.authGuard = authGuard;
    this.router = router;
  }

  public ngOnInit(): void {
    if (this.authGuard.isLoggedIn()) {
      this.paging$.subscribe((value) => this.refreshProjects(value));
    } else {
      this.router.navigate(['/login']);
    }
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
      this.service.getEverythingCategory().then(
        (win) => this.handleCategory(win),
        (fail) => this.handleError(fail)
      );

      return;
    }

    this.service
      .getProjectsPreview(
        paging.getCurrentPage() * environment.paging.maxProjects,
        environment.paging.maxProjects,
        paging.getSearchParam()
      )
      .then(
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
