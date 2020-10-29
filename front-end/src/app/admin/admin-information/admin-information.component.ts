import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

import { ResourcesService } from '../../shared/services/resources.service';
import { MessageType, MessageDescription } from '../../shared/message/message.component';
import { RequestResult } from '../../shared/models/request-result.interface';
import { Incident } from '../../shared/models/incident.interface';

import { Information } from '../information.interface';
import { PrivateService } from '../private.service';
import { AuthGuard } from '../../shared/services/auth.guard';

@Component({
  selector: 'app-admin-information',
  templateUrl: './admin-information.component.html',
  styleUrls: ['./admin-information.component.scss'],
})
export class AdminInformationComponent implements OnInit {
  public information$: BehaviorSubject<Information> = new BehaviorSubject<Information>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });

  public constructor(
    public textMessages: ResourcesService,
    private router: Router,
    private dataService: PrivateService,
    private authGuard: AuthGuard,
    titleService: Title,
  ) {
    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void {
    this.dataService.getInformation().subscribe(
      win => this.handleRequestResult(win),
      fail => this.handleError(fail)
    );
  }

  public logout(): void {
    this.authGuard.logoutComplete();
    this.router.navigate(['']);
  }

  private handleRequestResult(result: RequestResult<Information>): void {
    this.message$.next(null);

    if (result.isSucceed) {
      this.information$.next(result.data);
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
