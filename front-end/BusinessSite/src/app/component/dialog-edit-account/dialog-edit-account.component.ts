import { Component, Inject, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessageDescription, MessageType } from '../message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Account } from 'src/app/model/Account';

@Component({
  selector: 'app-dialog-edit-account.component',
  templateUrl: './dialog-edit-account.component.html',
  styleUrls: ['./dialog-edit-account.component.scss']
})

export class DialogEditAccountComponent implements OnInit
{
  public account$: BehaviorSubject<Account> = new BehaviorSubject<Account>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public title$: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  private accountId: number;
  private service: DataService;
  private dialog: MatDialogRef<DialogEditAccountComponent>;

  constructor(service: DataService, dialogRef: MatDialogRef<DialogEditAccountComponent>, @Inject(MAT_DIALOG_DATA) data: number)
  {
    this.service = service;
    this.dialog = dialogRef;
    this.accountId = data;
  }

  ngOnInit(): void
  {
    if (this.accountId)
    {
      this.service.getAccount(this.accountId)
                  .then
                  (
                    succeeded => this.handleAccount(succeeded),
                    rejected => this.handleError(rejected.message)
                  );
    }
    else
    {
      this.title$.next('Create new account');
      this.account$.next(new Account());
    }
  }

  public save(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: StaticNames.SaveInProgress, type: MessageType.Spinner });

    this.service.saveAccount(this.account$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: StaticNames.SaveComplete, type: MessageType.Info });
                    this.handleAccount(succeeded);
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public delete(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: StaticNames.DeleteInProgress, type: MessageType.Spinner });

    this.service.deleteAccount(this.account$.value)
                .then
                (
                  () =>
                  {
                    this.account$.next(null);
                    this.title$.next('Delete account');
                    this.message$.next({text: StaticNames.DeleteComplete, type: MessageType.Info });
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public close(): void
  {
    this.disableInput$.next(true);
    //this.dialog.close();
  }

  private handleAccount(result: RequestResult<Account>): void
  {
    this.disableInput$.next(false);

    if (result.isSucceed)
    {
      this.account$.next(result.data);
      this.title$.next('Edit "' + result.data.login + '" account');
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: string): void
  {
    this.disableInput$.next(false);
    this.message$.next({text: error, type: MessageType.Error });
  }

}
