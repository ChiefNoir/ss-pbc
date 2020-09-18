import { Component, Inject, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessageDescription, MessageType } from '../message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';
import { DataService } from 'src/app/service/data.service';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Account } from 'src/app/model/Account';

@Component({
  selector: 'app-dialog-edit-account.component',
  templateUrl: './dialog-edit-account.component.html',
  styleUrls: ['./dialog-edit-account.component.scss']
})

export class DialogEditAccountComponent implements OnInit
{
  public account$: BehaviorSubject<Account> = new BehaviorSubject<Account>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public roles$: BehaviorSubject<string[]> = new BehaviorSubject<string[]>(null);
  public title$: BehaviorSubject<string> = new BehaviorSubject<string>('Account properties');
  public staticNames: StaticNames = new StaticNames();

  private accountId: number;
  private service: DataService;
  private dialog: MatDialogRef<DialogEditAccountComponent>;

  constructor(service: DataService, dialogRef: MatDialogRef<DialogEditAccountComponent>, @Inject(MAT_DIALOG_DATA) data: number)
  {
    this.service = service;
    this.dialog = dialogRef;
    this.accountId = data;
  }

  public ngOnInit(): void
  {
    this.service.getRoles()
                .then
                (
                  succeeded => this.handleRoles(succeeded),
                  rejected => this.handleError(rejected.message)
                );

    if (this.accountId)
    {
      this.message$.next({type: MessageType.Spinner });
      this.service.getAccount(this.accountId)
                  .then
                  (
                    succeeded =>
                    {
                      this.message$.next({text: this.staticNames.LoadComplete, type: MessageType.Info });
                      this.handleAccount(succeeded);
                    },
                    rejected => this.handleError(rejected.message)
                  );
    }
    else
    {
      this.title$.next(this.staticNames.AccountCreate);
      this.disableInput$.next(false);
      this.account$.next(new Account());
    }
  }

  public save(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: this.staticNames.SaveInProgress, type: MessageType.Spinner });

    this.service.saveAccount(this.account$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: this.staticNames.SaveComplete, type: MessageType.Info });
                    this.handleAccount(succeeded);
                  },
                  rejected => this.handleError(rejected.message)
                );
  }

  public delete(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: this.staticNames.DeleteInProgress, type: MessageType.Spinner });

    this.service.deleteAccount(this.account$.value)
                .then
                (
                  win => this.handleDelete(win),
                  fail => this.handleError(fail)
                );
  }

  public close(): void
  {
    this.dialog.close();
  }

  private handleDelete(result: RequestResult<boolean>): void
  {
    if (result.isSucceed)
    {
      this.account$.next(null);
      this.title$.next(this.staticNames.AccountDelete);
      this.message$.next({text: this.staticNames.DeleteComplete, type: MessageType.Info });
    }
    else
    {
      this.handleError(result.error);
    }
  }

  private handleAccount(result: RequestResult<Account>): void
  {
    this.disableInput$.next(false);

    if (result.isSucceed)
    {
      this.account$.next(result.data);
      this.title$.next(this.staticNames.AccountEdit + '　”'　+　result.data.login + '”');
    }
    else
    {
      this.handleIncident(result.error);
    }
  }

  private handleRoles(result: RequestResult<string[]>): void
  {
    if (result.isSucceed)
    {
      this.roles$.next(result.data);
    }
    else
    {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void
  {
    this.disableInput$.next(false);

    console.log(error);
    this.message$.next({text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void
  {
    this.disableInput$.next(false);
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
