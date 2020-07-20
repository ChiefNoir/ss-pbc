import { Component, Input, AfterViewInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { MatDialogRef } from '@angular/material/dialog';
import { MessageDescription, MessageType } from '../message/message.component';
import { Account } from 'src/app/model/Account';

@Component({
  selector: 'app-dialog-edit-account.component',
  templateUrl: './dialog-edit-account.component.html',
  styleUrls: ['./dialog-edit-account.component.scss']
})

export class DialogEditAccountComponent implements AfterViewInit
{
  @Input()
  public id: number;

  private service: DataService;
  private dialog: MatDialogRef<DialogEditAccountComponent>;

  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public account$: BehaviorSubject<Account> = new BehaviorSubject<Account>(null);
  public loadingMessage: MessageDescription = {text: 'Loading', type: MessageType.Spinner };

  constructor(service: DataService, dialogRef: MatDialogRef<DialogEditAccountComponent>){
    this.service = service;
    this.dialog = dialogRef;
  }

  public ngAfterViewInit(): void
  {
    if(this.id) {
    this.service.getAccount(this.id)
                .then
                (
                  succeeded => this.handle(this.account$, succeeded),
                  rejected => this.handleError(rejected.message)
                );
    }
    else
    {
      this.account$.next(new Account());
    }
  }

  public save(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: 'Saving in progress', type: MessageType.Spinner  });

    this.service.saveAccount(this.account$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: 'Saving complete', type: MessageType.Info  }); 
                    this.handle(this.account$, succeeded);
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public delete(): void
  {
    this.message$.next({text: 'Deleting in progress', type: MessageType.Spinner  });
    this.disableInput$.next(true);

    this.service.deleteAccount(this.account$.value)
                .then
                (
                  () => this.close(),
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
