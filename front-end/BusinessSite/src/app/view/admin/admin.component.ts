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
import { AuthGuard } from 'src/app/guards/authGuard';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})

export class AdminComponent implements OnInit
{
  private router: Router;
  private authGuard: AuthGuard;
  public information$: BehaviorSubject<Information> = new BehaviorSubject<Information> (null);

  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });
  private dataService: DataService;
  

  public constructor(router: Router, titleService: Title, dataService: DataService, authGuard: AuthGuard)
  {
    this.router = router;
    this.dataService = dataService;
    this.authGuard = authGuard;

    titleService.setTitle(environment.siteName);
  }

  public async ngOnInit(): Promise<void>
  {
    await this.authGuard.checkIsLogged();
    if (this.authGuard.isLoggedIn$.value)
    {
      this.dataService.getInformation()
                      .then
                      (
                        result => this.handleRequestResult(result),
                        reject => this.handleError(reject)
                      );
    }
    else
    {
      this.router.navigate(['/login']);
    }
  }

  public logout(): void
  {
    this.authGuard.logoutComplete();
    this.router.navigate(['']);
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
