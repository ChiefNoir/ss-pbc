import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject, forkJoin  } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { MatDialog } from '@angular/material/dialog';
import { Account } from 'src/app/model/Account';

import { DialogEditAccountComponent } from 'src/app/component/dialog-edit-account/dialog-edit-account.component';
import { Paging } from 'src/app/model/PagingInfo';

@Component({
  selector: 'app-admin-edit-accounts',
  templateUrl: './admin-edit-accounts.component.html',
  styleUrls: ['./admin-edit-accounts.component.scss'],
})

export class AdminEditAccountsComponent implements AfterViewInit, OnDestroy {
  private service: DataService;

  public accounts$: BehaviorSubject<Array<Account>> = new BehaviorSubject<Array<Account>>(null);
  public paging$: BehaviorSubject<Paging> = new BehaviorSubject<Paging>(null);
  public dialog: MatDialog;
  public columns: string[] = ['login', 'role'];

  public constructor(service: DataService, titleService: Title, dialog: MatDialog) {
    this.service = service;
    this.dialog = dialog;

    titleService.setTitle(environment.siteName);
    this.paging$.subscribe(value => this.refreshProjects(value));
  }

  ngAfterViewInit(): void
  {
    this.refreshProjects(null);
  }

  ngOnDestroy(): void
  {
    this.paging$.unsubscribe();
  }

  private refreshPaging(): void
  {
    this.service.countAccount()
                .then
                (
                  response => this.hanlePaging(response, this.paging$),
                  reject => this.handleError(reject)
                );
  }

  private refreshProjects(paging: Paging): void
  {
    if (!paging)
    {
      this.refreshPaging();
      return;
    }

    this.service.getAccounts(paging.getCurrentPage() * environment.paging.maxUsers, environment.paging.maxUsers)
                .then
                (
                  response => this.handle(response, this.accounts$),
                  reject =>  this.handleError(reject)
                );
  }

  public changePage(page: number): void
  {
    this.paging$.next(new Paging(page, environment.paging.maxUsers, this.paging$.value.getMaxItems()));
  }

  public skipPage(amount: number): void
  {
    this.changePage(this.paging$.value.getCurrentPage() + amount);
  }

  public showDialog(id?: number): void {
    const dialogRef = this.dialog.open(DialogEditAccountComponent, {width: '50%'});

    if (id) {
    dialogRef.componentInstance.id = id;
    }

    dialogRef.afterClosed()
             .subscribe
             (
               () => this.changePage(this.paging$.value.getCurrentPage())
             );
  }

  private hanlePaging(result: RequestResult<number>, content: BehaviorSubject<Paging>): void
  {
    if (result.isSucceed)
    {
      const currentPage = content?.value?.getCurrentPage() ?? 0;
      content.next(new Paging(currentPage, environment.paging.maxUsers, result.data));
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handle<T>(result: RequestResult<T>, content: BehaviorSubject<T>): void {
    if (result.isSucceed) {
    content.next(result.data);
    } else{
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
