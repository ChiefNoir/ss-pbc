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
  private categoryId: number;
  private service: DataService;
  private dialog: MatDialogRef<DialogEditorCategoryComponent>;

  public category$: BehaviorSubject<Category> = new BehaviorSubject<Category>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public title$: BehaviorSubject<string> = new BehaviorSubject<string>('Category properties');
  public staticNames: StaticNames = new StaticNames();

  public systemCategoryMessage: MessageDescription = {text: this.staticNames.CategorySystemWarning, type: MessageType.Info };

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
                    succeeded => this.handleCategory(succeeded, {text: this.staticNames.LoadComplete, type: MessageType.Info }),
                    rejected => this.handleError(rejected.message)
                  );
    }
    else
    {
      this.disableInput$.next(false);
      this.category$.next(new Category());
    }
  }


  public save(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: this.staticNames.SaveInProgress, type: MessageType.Spinner  });

    this.service.saveCategory(this.category$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: this.staticNames.SaveInProgress, type: MessageType.Info });
                    this.handleCategory(succeeded, {text: this.staticNames.SaveComplete, type: MessageType.Info });
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public delete(): void
  {
    this.message$.next({text: this.staticNames.DeleteInProgress, type: MessageType.Spinner  });
    this.disableInput$.next(true);

    this.service.deleteCategory(this.category$.value)
                .then
                (
                  succeeded => this.handleCategory(succeeded, {text: this.staticNames.DeleteComplete, type: MessageType.Info }),
                  rejected => this.handleError(rejected.message)
                );
  }

  public close(): void
  {
    this.dialog.close();
  }

  private handleCategory(result: RequestResult<Category>, description: MessageDescription): void
  {
    this.disableInput$.next(false);

    if (result.isSucceed)
    {
      this.category$.next(result.data);
      this.title$.next('Edit "' + result.data.code + '" category');
      this.message$.next(description);
    }
    else
    {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void
  {
    console.log(error);

    this.disableInput$.next(false);
    this.message$.next({text: error.detail, type: MessageType.Error });
  }

  private handleError(error: any): void
  {
    console.log(error);

    this.disableInput$.next(false);

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
