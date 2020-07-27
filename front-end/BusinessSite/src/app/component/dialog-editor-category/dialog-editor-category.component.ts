import { Component, Input, AfterViewInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { MatDialogRef } from '@angular/material/dialog';
import { MessageDescription, MessageType } from '../message/message.component';

@Component({
  selector: 'app-dialog-editor-category.component',
  templateUrl: './dialog-editor-category.component.html',
  styleUrls: ['./dialog-editor-category.component.scss']
})

export class DialogEditorCategoryComponent implements AfterViewInit
{
  @Input()
  public categoryId: number;

  private service: DataService;
  private dialog: MatDialogRef<DialogEditorCategoryComponent>;

  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public category$: BehaviorSubject<Category> = new BehaviorSubject<Category>(null);
  public loadingMessage: MessageDescription = {text: 'Loading', type: MessageType.Spinner };

  constructor(service: DataService, dialogRef: MatDialogRef<DialogEditorCategoryComponent>){
    this.service = service;
    this.dialog = dialogRef;
  }

  public ngAfterViewInit(): void
  {
    if (this.categoryId)
    {
    this.service.getCategory(this.categoryId)
                .then
                (
                  succeeded => this.handle(this.category$, succeeded),
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

    this.service.save(this.category$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: 'Saving complete', type: MessageType.Info  }); 
                    this.handle(this.category$, succeeded);
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public delete(): void
  {
    this.message$.next({text: 'Deleting in progress', type: MessageType.Spinner  });
    this.disableInput$.next(true);

    this.service.delete(this.category$.value)
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

  private handle<T>(content: BehaviorSubject<T>, result: RequestResult<T>): void
  {
    this.disableInput$.next(false);

    if (result.isSucceed)
    {
      content.next(result.data);
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: string): void
  {
    this.disableInput$.next(false);
    this.message$.next({text: error, type: MessageType.Error  });
    console.log(error);
  }
}
