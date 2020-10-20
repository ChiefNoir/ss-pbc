import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { Title } from '@angular/platform-browser';
import {
  MessageDescription,
  MessageType,
} from 'src/app/shared/message/message.component';
import { ResourcesService } from 'src/app/core/resources.service';
import { Information } from 'src/app/admin/information.model';
import { environment } from 'src/environments/environment';
import { RequestResult, Incident } from 'src/app/shared/request-result.model';
import { AuthGuard } from 'src/app/core/auth.guard';
import { PrivateService } from 'src/app/core/private.service';

@Component({
  selector: 'app-admin-information',
  templateUrl: './admin-information.component.html',
  styleUrls: ['./admin-information.component.scss'],
})
export class AdminInformationComponent implements OnInit {
  public information$: BehaviorSubject<Information> = new BehaviorSubject<
    Information
  >(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<
    MessageDescription
  >({ type: MessageType.Spinner });

  public constructor(
    private router: Router,
    titleService: Title,
    public textMessages: ResourcesService,
    private dataService: PrivateService,
    private authGuard: AuthGuard
  ) {
    titleService.setTitle(environment.siteName);
  }

  public async ngOnInit(): Promise<void> {
    if (this.authGuard.isLoggedIn()) {
      this.dataService.getInformation().then(
        (result) => this.handleRequestResult(result),
        (reject) => this.handleError(reject)
      );
    } else {
      this.router.navigate(['/login']);
    }
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
    console.log(error);
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    console.log(error);

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
