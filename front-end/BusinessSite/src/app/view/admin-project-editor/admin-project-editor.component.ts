import { Component } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Project } from 'src/app/model/Project';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ProjectEditorComponent } from 'src/app/component/project-editor/project-editor.component';

@Component({
  selector: 'app-admin-project-editor',
  templateUrl: './admin-project-editor.component.html',
  styleUrls: ['./admin-project-editor.component.scss']
})

export class AdminProjectEditorComponent implements AfterViewInit {

  private service: DataService;
  public projects$: BehaviorSubject<Array<Project>> = new BehaviorSubject<Array<Project>>(null);
  public dialog: MatDialog;

  private activeRoute: ActivatedRoute;
  private projectsPerPage: number = environment.maxProjectsPerPage;

  public columns: string[] = ['code', 'displayName', 'category'];

  public constructor(route: ActivatedRoute, router: Router, service: DataService, dialog: MatDialog) {
    this.service = service;
    this.activeRoute = route;
    this.dialog = dialog;
  }

  ngAfterViewInit() {
    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });
  }


  private refreshPage(): void {
    this.service.getProjects(1 * this.projectsPerPage, this.projectsPerPage, 'all')
    .then
    (
      result => this.handle(result, this.projects$),
      error => this.handleError(error)
    );
  }

  private handle<T>(data: RequestResult<T>, content: BehaviorSubject<T>): void {
    // TODO: handle internal error
    content.next(data.data);
  }

  private handleError(error: any): void {
    console.log(error);
  }

  public showRow(data: any): void{
    const dialogRef = this.dialog.open(ProjectEditorComponent, {

      width: '70%',
    });

    dialogRef.componentInstance.code = data;


    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}

