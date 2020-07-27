import { Component, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditorCategoryComponent } from 'src/app/component/dialog-editor-category/dialog-editor-category.component';

@Component({
  selector: 'app-admin-edit-categories',
  templateUrl: './admin-edit-categories.component.html',
  styleUrls: ['./admin-edit-categories.component.scss'],
})

export class AdminEditCategoriesComponent implements AfterViewInit {
  private service: DataService;
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);
  public dialog: MatDialog;

  //public columns: string[] = ['code', 'displayName', 'isEverything'];
  private columnDefinitions = [
    { def: 'id', show: false },
    { def: 'code', show: true },
    { def: 'displayName', show: true },
    { def: 'isEverything', show: true },
  ];

  public constructor(service: DataService, titleService: Title, dialog: MatDialog) {
    this.service = service;
    this.dialog = dialog;

    titleService.setTitle(environment.siteName);
  }

  ngAfterViewInit(): void {
    this.service.getCategories()
                .then
                (
                  (result) => this.handle(result, this.categories$),
                  (error) => this.handleError(error)
                );
  }

  public showCreator(): void
  {
    const dialogRef = this.dialog.open(DialogEditorCategoryComponent, {width: '50%'});
    dialogRef.afterClosed()
             .subscribe
             (
               (result) =>
               {
                this.service.getCategories()
                .then
                (
                  (x) => this.handle(x, this.categories$),
                  (error) => this.handleError(error)
                );
                }
            );
  }

  public getDisplayedColumns(): string[] {
    return this.columnDefinitions
      .filter(cd => cd.show)
      .map(cd => cd.def);
  }

  public showEditor(categoryId: number): void {
    const dialogRef = this.dialog.open(DialogEditorCategoryComponent, {width: '50%'});

    if(categoryId) {
    dialogRef.componentInstance.categoryId = categoryId;
    }
    
    dialogRef.afterClosed()
             .subscribe
             (
               (result) =>
               {
                this.service.getCategories()
                .then
                (
                  (x) => {
                    this.handle(x, this.categories$)
                  },
                  (error) => this.handleError(error)
                );
                }
            );
  }

  private handle<T>(result: RequestResult<T>, content: BehaviorSubject<T>): void {
    if (result.isSucceed) {
    content.next(result.data);
    } else{
      this.handleError(result.errorMessage);
    }
  }
  
  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
