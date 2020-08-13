import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { StorageService } from 'src/app/service/storage.service';

import { Title } from '@angular/platform-browser';
import { MessageDescription, MessageType } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';
import { DataService } from 'src/app/service/data.service';
import { Information } from 'src/app/model/Information';
import { environment } from 'src/environments/environment';
import { RequestResult } from 'src/app/model/RequestResult';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})

export class AdminComponent implements OnInit
{
  private router: Router;
  public information$: BehaviorSubject<Information> = new BehaviorSubject<Information> (null);

  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });
  private dataService: DataService;
  private storage: StorageService;

  public constructor(router: Router, titleService: Title, dataService: DataService, storage: StorageService)
  {
    this.router = router;
    this.dataService = dataService;
    this.storage = storage;


    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void
  {

    this.dataService.getInformation(this.storage .getToken())
    .then
    (
      ok => this.handleInformation(ok),
      fail => this.handleError(fail)
    );
  }

  public logout(): void
  {

  }

  private handleInformation(result: RequestResult<Information>): void
  {
    if (result.isSucceed)
    {
      this.message$.next(null);
      this.information$.next(result.data);
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void
  {
    this.message$.next({text: error, type: MessageType.Error  });
    console.log(error);
  }

}
