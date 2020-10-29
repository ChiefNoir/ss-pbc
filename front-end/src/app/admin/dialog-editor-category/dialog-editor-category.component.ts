import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BehaviorSubject } from 'rxjs';

import { PublicService } from '../../shared/services/public.service';
import { ResourcesService } from '../../shared/services/resources.service';
import { RequestResult } from '../../shared/models/request-result.interface';
import { Incident } from '../../shared/models/incident.interface';
import { Category } from '../../shared/models/category.model';
import { MessageDescription, MessageType } from '../../shared/message/message.component';

import { PrivateService } from '../private.service';

@Component({
  selector: 'app-dialog-editor-category.component',
  templateUrl: './dialog-editor-category.component.html',
  styleUrls: ['./dialog-editor-category.component.scss'],
})
export class DialogEditorCategoryComponent implements OnInit {
  private categoryId: number;

  public category$: BehaviorSubject<Category> = new BehaviorSubject<Category>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);

  constructor(
    private service: PrivateService,
    public textMessages: ResourcesService,
    private publicService: PublicService,
    private dialog: MatDialogRef<DialogEditorCategoryComponent>,
    @Inject(MAT_DIALOG_DATA) categoryId: number
  ) {
    this.categoryId = categoryId;
  }

  public ngOnInit(): void {
    if (this.categoryId) {
      this.publicService
          .getCategory(this.categoryId)
          .subscribe(
            win => this.handleCategory(win, {text: this.textMessages.LoadComplete, type: MessageType.Info }),
            fail => this.handleError(fail.message));
    } else {
      this.disableInput$.next(false);
      this.category$.next(new Category());
    }
  }

  public save(): void {
    this.disableInput$.next(true);
    this.message$.next({text: this.textMessages.SaveInProgress, type: MessageType.Spinner});

    this.service
        .saveCategory(this.category$.value)
        .subscribe(
          win => {
            this.message$.next({ text: this.textMessages.SaveInProgress, type: MessageType.Info});
            this.handleCategory(win, {text: this.textMessages.SaveComplete, type: MessageType.Info}); },
          fail => this.handleError(fail.message)
    );
  }

  public delete(): void {
    this.message$.next({text: this.textMessages.DeleteInProgress, type: MessageType.Spinner });
    this.disableInput$.next(true);

    this.service
        .deleteCategory(this.category$.value)
        .subscribe(
          win => this.handleDelete(win),
          fail => this.handleError(fail)
        );
  }

  public close(): void {
    this.dialog.close();
  }

  private handleDelete(result: RequestResult<boolean>): void {
    if (result.isSucceed) {
      this.category$.next(null);
      this.message$.next({text: this.textMessages.DeleteComplete, type: MessageType.Info});
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleCategory(result: RequestResult<Category>, description: MessageDescription): void {
    this.disableInput$.next(false);

    if (result.isSucceed) {
      this.category$.next(result.data);
      this.message$.next(description);
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    this.disableInput$.next(false);
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    this.disableInput$.next(false);

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
