import { Component, Inject, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { StaticNames } from 'src/app/common/StaticNames';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessageDescription, MessageType } from '../message/message.component';

@Component({
  selector: 'app-dialog-editor-category.component',
  templateUrl: './dialog-editor-category.component.html',
  styleUrls: ['./dialog-editor-category.component.scss']
})

export class DialogEditorCategoryComponent implements OnInit
{
  public category$: BehaviorSubject<Category> = new BehaviorSubject<Category>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public systemCategoryMessage: MessageDescription = {text: 'You can\'t delete system category', type: MessageType.Info };
  public title$: BehaviorSubject<string> = new BehaviorSubject<string>('Category properties');

  private categoryId: number;
  private service: DataService;
  private dialog: MatDialogRef<DialogEditorCategoryComponent>;

  constructor(service: DataService, dialogRef: MatDialogRef<DialogEditorCategoryComponent>, @Inject(MAT_DIALOG_DATA) categoryId: number)
  {
    this.service = service;
    this.dialog = dialogRef;
    this.categoryId = categoryId;
  }

  public ngOnInit(): void
  {
    if (this.categoryId)
    {
      this.service.getCategory(this.categoryId)
                  .then
                  (
                    succeeded => this.handleCategory(succeeded),
                    rejected => this.handleError(rejected.message)
                  );
    }
    else
    {
      this.category$.next(new Category());
    }
  }


  public save(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: 'Saving in progress', type: MessageType.Spinner  });

    this.service.saveCategory(this.category$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: 'Saving complete', type: MessageType.Info  });
                    this.handleCategory(succeeded);
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public delete(): void
  {
    this.message$.next({text: 'Deleting in progress', type: MessageType.Spinner  });
    this.disableInput$.next(true);

    this.service.deleteCategory(this.category$.value)
                .then
                (
                  succeeded => this.close(),
                  rejected => this.handleError(rejected.message)
                );
  }

  public close(): void
  {
    this.dialog.close();
  }

  private handleCategory(result: RequestResult<Category>): void
  {
    this.disableInput$.next(false);

    if (result.isSucceed)
    {
      this.category$.next(result.data);
      this.title$.next('Edit "' + result.data.code + '" category');
      this.message$.next({text: StaticNames.LoadComplete, type: MessageType.Info });
    }
    else
    {
      this.handleError(result.error);
    }
  }

  private handleError(error: any): void
  {
    this.disableInput$.next(false);

    if (error instanceof Incident)
    {
      this.message$.next({text: error.code + '<br/>' + error.detail + '<br/>' + error.message, type: MessageType.Error });
      return;
    }

    this.message$.next({text: error, type: MessageType.Error });
  }

}
