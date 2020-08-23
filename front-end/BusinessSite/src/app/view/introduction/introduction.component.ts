import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Introduction } from 'src/app/model/Introduction';
import { MessageDescription, MessageType } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';

@Component({
  selector: 'app-introduction',
  templateUrl: './introduction.component.html',
  styleUrls: ['./introduction.component.scss'],
})

export class IntroductionComponent implements OnInit {
  private service: DataService;

  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });
  public constructor(service: DataService, titleService: Title) {
    this.service = service;

    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void
  {
    this.service.getIntroduction()
                .then
                (
                  result =>  this.handle(result),
                  reject => this.handleError(reject)
                );
  }

  private handle(result: RequestResult<Introduction>): void
  {
    if (result.isSucceed)
    {
      this.introduction$.next(result.data);

      if (!result.data)
      {
        this.message$.next({text: 'Nothing was found', type: MessageType.Spinner  });
      }
      else
      {
        this.message$.next(null);
      }
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
