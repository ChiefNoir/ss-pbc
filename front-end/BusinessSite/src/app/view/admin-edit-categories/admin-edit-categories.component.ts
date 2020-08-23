import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditorCategoryComponent } from 'src/app/component/dialog-editor-category/dialog-editor-category.component';
import { MessageType, MessageDescription } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';

@Component({
  selector: 'app-admin-edit-categories',
  templateUrl: './admin-edit-categories.component.html',
  styleUrls: ['./admin-edit-categories.component.scss'],
})

export class AdminEditCategoriesComponent implements OnInit
{
  private service: DataService;
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });
  public dialog: MatDialog;

  private columnDefinitions = [
    { def: 'id', show: false },
    { def: 'code', show: true },
    { def: 'displayName', show: true },
    { def: 'isEverything', show: true },
  ];

  public constructor(service: DataService, titleService: Title, dialog: MatDialog)
  {
    this.service = service;
    this.dialog = dialog;

    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void
  {
    this.service.getCategories()
                .then
                (
                  (result) => this.handleCategories(result),
                  (error) => this.handleError(error)
                );
  }

  public showCreator(): void
  {
    const dialogRef = this.dialog.open(DialogEditorCategoryComponent, {width: '50%'});

    dialogRef.afterClosed()
             .subscribe
             (
               () =>{
                      this.service.getCategories()
                                  .then
                                  (
                                    result => this.handleCategories(result),
                                    reject => this.handleError(reject)
                                  );
                    }
             );
  }

  public getDisplayedColumns(): string[]
  {
    return this.columnDefinitions
                .filter(x => x.show)
                .map(x => x.def);
  }

  public showEditor(categoryId: number): void
  {
    const dialogRef = this.dialog.open(DialogEditorCategoryComponent, {width: '50%', data: categoryId});

    dialogRef.afterClosed()
             .subscribe
             (
               () => {
                      this.service.getCategories()
                                  .then
                                  (
                                    result => this.handleCategories(result),
                                    reject => this.handleError(reject)
                                  );
                      }
            );
  }

  private handleCategories(result: RequestResult<Category[]>): void
  {
    if (result.isSucceed)
    {
      this.categories$.next(result.data);
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
