import { Component } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { MatDialog } from '@angular/material/dialog';
import { ProjectEditorComponent } from 'src/app/component/project-editor/project-editor.component';
import { PagingInfo } from 'src/app/model/PagingInfo';
import { Category } from 'src/app/model/Category';
import { ProjectPreview } from 'src/app/model/ProjectPreview';
import { Project } from 'src/app/model/Project';
import { MessageType, MessageDescription } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';

@Component({
  selector: 'app-admin-edit-projects',
  templateUrl: './admin-edit-projects.component.html',
  styleUrls: ['./admin-edit-projects.component.scss'],
})

export class AdminEditProjectsComponent implements AfterViewInit {
  private service: DataService;
  private currentPage: number = 0;
  private maxPage: number = 0;
  private minPage: number = 0;
  public columns: string[] = ['code', 'displayName', 'category', 'releaseDate'];

  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);
  public pagingInfo$: BehaviorSubject<PagingInfo> = new BehaviorSubject<PagingInfo>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });
  public dialog: MatDialog;

  public constructor(service: DataService, dialog: MatDialog) {
    this.service = service;
    this.dialog = dialog;
  }

  ngAfterViewInit() {
    this.service.getEverythingCategory()
                .then
                (
                  (result) => this.hadleCategory(result),
                  (error) => this.handleError(error)
                );
  }

  public showRow(data: any): void {
    const dialogRef = this.dialog.open(ProjectEditorComponent, {width: '90%', minHeight:'80%'});

    dialogRef.componentInstance.code = data;

    dialogRef.afterClosed()
             .subscribe
             (
               (result) =>
               {
                 this.changePage(this.currentPage);
                }
            );
  }

  public nextPage() {
    this.changePage(this.currentPage + 1);
  }

  public backPage() {
    this.changePage(this.currentPage - 1);
  }

  public changePage(page: number): void {
    if (!page) {
      page = this.minPage;
    }
    if (page > this.maxPage) {
      page = this.maxPage;
    }
    if (page < this.minPage) {
      page = this.minPage;
    }

    this.currentPage = page;
    this.projects$.next(null);

    this.pagingInfo$.next
    ({
        minPage: this.minPage,
        maxPage: Math.ceil(this.maxPage / environment.maxProjectsPerPage) - 1,
        currentPage: this.currentPage
    });

    this.service.getProjectsPreview(this.currentPage * environment.maxProjectsPerPage, environment.maxProjectsPerPage, '')
                .then
                (
                  (result) => this.handle(result, this.projects$),
                  (error) => this.handleError(error)
                );
  }

  private hadleCategory(result: RequestResult<Category>) {
    if (result.isSucceed) {
      this.currentPage = 0;
      this.maxPage = Math.ceil(this.maxPage / environment.maxProjectsPerPage) - 1;
      this.minPage = 0;
      this.changePage(this.currentPage);
    } else {
      this.handleError(result.errorMessage);
    }
  }

  private handle<T>(result: RequestResult<T>, content: BehaviorSubject<T>): void {
    if (result.isSucceed) {
    content.next(result.data);
    } else{
      this.handleError(result.errorMessage);
    }
  }



  private handleError(error: any): void {
    console.log(error);
  }

}
