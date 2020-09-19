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
import { TextMessages } from 'src/app/resources/TextMessages';
import { AuthGuard } from 'src/app/guards/authGuard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-edit-categories',
  templateUrl: './admin-edit-categories.component.html',
  styleUrls: ['./admin-edit-categories.component.scss'],
})

export class AdminEditCategoriesComponent implements OnInit
{
  private service: DataService;
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });
  public dialog: MatDialog;
  private authGuard: AuthGuard;
  private router: Router;
  public textMessages: TextMessages = new TextMessages();

  private columnDefinitions = [
    { def: 'id', show: false },
    { def: 'code', show: true },
    { def: 'displayName', show: true },
    { def: 'isEverything', show: true },
  ];

  public constructor(service: DataService, titleService: Title, dialog: MatDialog, authGuard: AuthGuard, router: Router)
  {
    this.service = service;
    this.dialog = dialog;
    this.authGuard = authGuard;
    this.router = router;

    titleService.setTitle(environment.siteName);
  }

  public async ngOnInit(): Promise<void>
  {
    await this.authGuard.checkIsLogged();
    if (this.authGuard.isLoggedIn$.value)
    {
      this.refreshCategories();
    }
    else
    {
      this.router.navigate(['/login']);
    }
  }

  public getDisplayedColumns(): string[]
  {
    return this.columnDefinitions
                .filter(x => x.show)
                .map(x => x.def);
  }

  public showDialog(categoryId?: number): void
  {
    const dialogRef = this.dialog.open(DialogEditorCategoryComponent, {width: '50%', data: categoryId});

    dialogRef.afterClosed()
             .toPromise()
             .then
             (
               () => this.refreshCategories()
             );
  }

  private refreshCategories(): void
  {
    // there is no paging in the categories, because there will be not many categories
    this.service.getCategories()
                .then
                (
                  win => this.handleCategories(win),
                  fail => this.handleError(fail)
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
    console.log(error);
    this.message$.next({text: error.message, type: MessageType.Error });
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
