import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject } from 'rxjs';

import { RequestResult } from '../../shared/request-result.interface';
import { Incident } from '../../shared/incident.interface'
import { MessageDescription, MessageType } from '../../shared/message/message.component';

import { Account } from '../account.model';

import { DialogEditAccountComponent } from '../dialog-edit-account/dialog-edit-account.component';
import { Paging } from '../../shared/paging-info.model';

import { ResourcesService } from '../../core/services/resources.service';
import { PrivateService } from '../private.service';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-accounts',
  templateUrl: './admin-accounts.component.html',
  styleUrls: ['./admin-accounts.component.scss'],
})
export class AdminAccountsComponent implements OnInit, OnDestroy {
  public accounts$: BehaviorSubject<Array<Account>> = new BehaviorSubject<Array<Account>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });
  public paging$: BehaviorSubject<Paging<null>> = new BehaviorSubject<Paging<null>>(null);
  public columns: string[] = ['login', 'role'];

  public constructor(
    public textMessages: ResourcesService,
    public dialog: MatDialog,
    private service: PrivateService,
    titleService: Title
  ) {
    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void {
    this.refreshAccounts(null);
    this.paging$.subscribe((value) => this.refreshAccounts(value));
  }

  public ngOnDestroy(): void {
    this.paging$.unsubscribe();
  }

  private refreshPaging(): void {
    this.service.countAccount().subscribe(
      (win) => this.handlePaging(win, this.paging$),
      (fail) => this.handleError(fail)
    );
  }

  private refreshAccounts(paging: Paging<null>): void {
    if (!paging) {
      this.refreshPaging();
      return;
    }

    this.service
      .getAccounts(
        paging.getCurrentPage() * environment.paging.maxUsers,
        environment.paging.maxUsers
      )
      .subscribe(
        (result) => this.handleAccounts(result),
        (reject) => this.handleError(reject)
      );
  }

  public changePage(page: number): void {
    this.paging$.next(
      new Paging(
        page,
        environment.paging.maxUsers,
        this.paging$.value.getMaxItems()
      )
    );
  }

  public skipPage(amount: number): void {
    this.changePage(this.paging$.value.getCurrentPage() + amount);
  }

  public showDialog(id?: number): void {
    const dialogRef = this.dialog.open(DialogEditAccountComponent, {
      width: '50%',
      data: id,
    });

    dialogRef
      .afterClosed()
      .toPromise()
      .then(
        // TODO: it is a full refresh
        () => this.refreshAccounts(this.paging$.value)
      );
  }

  private handlePaging(
    result: RequestResult<number>,
    content: BehaviorSubject<Paging<null>>
  ): void {
    if (result.isSucceed) {
      const currentPage = content?.value?.getCurrentPage() ?? 0;
      content.next(
        new Paging(currentPage, environment.paging.maxUsers, result.data)
      );
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleAccounts(result: RequestResult<Account[]>): void {
    this.message$.next(null);
    if (result.isSucceed) {
      this.accounts$.next(result.data);
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
