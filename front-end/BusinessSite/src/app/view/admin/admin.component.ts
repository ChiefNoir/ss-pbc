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
import { RequestResult, Incident } from 'src/app/model/RequestResult';

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
  

  public constructor(router: Router, titleService: Title, dataService: DataService)
  {
    this.router = router;
    this.dataService = dataService;

    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void
  {
    this.dataService.getInformation()
                    .then
                    (
                      result => this.handleRequestResult(result),
                      reject => this.handleError(reject)
                    );
  }

  public logout(): void
  {

  }

  private handleRequestResult(result: RequestResult<Information>): void
  {
    this.message$.next(null);

    if (result.isSucceed)
    {
      this.information$.next(result.data);
    }
    else
    {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void
  {
    this.message$.next({text: error.code + ' : ' + error.message + '<br/>' + error.detail + '<br/>' , type: MessageType.Error });
  }

  private handleError(error: any): void
  {
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
