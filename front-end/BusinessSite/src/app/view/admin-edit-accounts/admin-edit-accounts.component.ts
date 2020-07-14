import { Component, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { MatDialog } from '@angular/material/dialog';
import { Account } from 'src/app/model/Account';

import { DialogEditorCategoryComponent } from 'src/app/component/dialog-editor-category/dialog-editor-category.component';
import { DialogEditAccountComponent } from 'src/app/component/dialog-edit-account/dialog-edit-account.component';

@Component({
  selector: 'app-admin-edit-accounts',
  templateUrl: './admin-edit-accounts.component.html',
  styleUrls: ['./admin-edit-accounts.component.scss'],
})

export class AdminEditAccountsComponent implements AfterViewInit {
  private service: DataService;
  public accounts$: BehaviorSubject<Array<Account>> = new BehaviorSubject<Array<Account>>(null);
  public dialog: MatDialog;

  public columns: string[] = ['login', 'role'];


  public constructor(service: DataService, titleService: Title, dialog: MatDialog) {
    this.service = service;
    this.dialog = dialog;

    titleService.setTitle(environment.siteName);
  }

  ngAfterViewInit(): void {

    this.service.countAccount('')
                .then
                (
                  result =>
                  {
                    this.loadAccount(result.data);
                  },
                  error => this.handleError(error)
                );
  }


  private loadAccount(max: number) : void
  {
this.service.getAccounts(0, 1, '')
                        .then
                        (
                          (x) => this.handle(x, this.accounts$),
                          (error) =>  this.handleError(error)
                );
  }


  public showRow(id: number): void {
    const dialogRef = this.dialog.open(DialogEditAccountComponent, {width: '50%'});

    if(id) {
    dialogRef.componentInstance.id = id;
    }

    dialogRef.afterClosed()
             .subscribe
             (
               (result) =>
               {
                this.service.getAccounts(0, 1, '')
                .then
                (
                  (x) => this.handle(x, this.accounts$),
                  (error) => this.handleError(error)
                );
                }
            );
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
