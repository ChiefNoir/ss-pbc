import { Component, Inject, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessageDescription, MessageType } from '../../shared/message/message.component';
import { ResourcesService } from '../../core/services/resources.service';
import { RequestResult } from '../../shared/request-result.interface';
import { Incident } from '../../shared/incident.interface';
import { Account } from '../account.model';
import { PrivateService } from '../private.service';

@Component({
  selector: 'app-dialog-edit-account.component',
  templateUrl: './dialog-edit-account.component.html',
  styleUrls: ['./dialog-edit-account.component.scss']
})

export class DialogEditAccountComponent implements OnInit {
  public account$: BehaviorSubject<Account> = new BehaviorSubject<Account>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public roles$: BehaviorSubject<string[]> = new BehaviorSubject<string[]>(null);
  public title$: BehaviorSubject<string> = new BehaviorSubject<string>('Account properties');

  private accountId: number;

  constructor(
    public textMessages: ResourcesService,
    private privateService: PrivateService,
    private dialog: MatDialogRef<DialogEditAccountComponent>,
    @Inject(MAT_DIALOG_DATA) data: number)
  {
    this.accountId = data;
  }

  public ngOnInit(): void {
    this.privateService
        .getRoles()
        .subscribe(
          win => this.handleRoles(win),
          fail => this.handleError(fail.message));

    if (this.accountId) {
      this.message$.next({type: MessageType.Spinner });
      this.privateService
          .getAccount(this.accountId)
          .subscribe(
            win => {
              this.message$.next({text: this.textMessages.LoadComplete, type: MessageType.Info });
              this.handleAccount(win);},
            rejected => this.handleError(rejected.message));
    }
    else {
      this.title$.next(this.textMessages.AccountCreate);
      this.disableInput$.next(false);
      this.account$.next(new Account());
    }
  }

  public save(): void {
    this.disableInput$.next(true);
    this.message$.next({text: this.textMessages.SaveInProgress, type: MessageType.Spinner });

    this.privateService
        .saveAccount(this.account$.value)
        .subscribe(
          win => {
            this.message$.next({text: this.textMessages.SaveComplete, type: MessageType.Info });
            this.handleAccount(win);},
          fail => this.handleError(fail.message));
  }

  public delete(): void {
    this.disableInput$.next(true);
    this.message$.next({text: this.textMessages.DeleteInProgress, type: MessageType.Spinner });

    this.privateService
        .deleteAccount(this.account$.value)
        .subscribe(
          win => this.handleDelete(win),
          fail => this.handleError(fail));
  }

  public close(): void {
    this.dialog.close();
  }

  private handleDelete(result: RequestResult<boolean>): void {
    if (result.isSucceed) {
      this.account$.next(null);
      this.title$.next(this.textMessages.AccountDelete);
      this.message$.next({text: this.textMessages.DeleteComplete, type: MessageType.Info });
    }
    else {
      this.handleError(result.error);
    }
  }

  private handleAccount(result: RequestResult<Account>): void {
    this.disableInput$.next(false);

    if (result.isSucceed) {
      this.account$.next(result.data);
      this.title$.next(this.textMessages.AccountEdit + '　”'　+　result.data.login + '”');
    }
    else {
      this.handleIncident(result.error);
    }
  }

  private handleRoles(result: RequestResult<string[]>): void {
    if (result.isSucceed) {
      this.roles$.next(result.data);
    }
    else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    this.disableInput$.next(false);

    this.message$.next({text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    this.disableInput$.next(false);

    if (error.name !== undefined) {
      this.message$.next({text: error.name, type: MessageType.Error });
    }
    else {
      this.message$.next({text: error, type: MessageType.Error });
    }
  }
}
