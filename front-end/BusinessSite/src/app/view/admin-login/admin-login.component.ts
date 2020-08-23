import { Component } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { RequestResult, Incident } from 'src/app/model/RequestResult';

import { AuthService } from 'src/app/service/auth.service';
import { Title } from '@angular/platform-browser';

import { environment } from 'src/environments/environment';
import { Identity } from 'src/app/model/Identity';
import { StorageService } from 'src/app/service/storage.service';
import { AuthGuard } from 'src/app/guards/authGuard';
import { MessageDescription, MessageType } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';


@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss'],
})

export class AdminLoginComponent {
  private router: Router;
  private authGuard: AuthGuard;
  private storageService: StorageService;


  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });
  private authService: AuthService;

  public login: FormControl = new FormControl('', [Validators.required]);
  public password: FormControl = new FormControl('', [Validators.required]);

  public constructor(authService: AuthService, authGuard: AuthGuard, router: Router, titleService: Title, storageService: StorageService)
  {
    this.authService = authService;
    this.router = router;
    this.storageService = storageService;
    this.authGuard = authGuard;

    titleService.setTitle(environment.siteName);
  }

  public doLogin(): void
  {
    if (!this.login.valid || !this.password.valid) { return; }

    this.authService.login(this.login.value, this.password.value)
                    .then
                    (
                      result => this.handleLoginResult(result),
                      reject => this.handleError(reject)
                    );
  }

  private handleLoginResult(result: RequestResult<Identity>): void
  {
    if (result.isSucceed)
    {
      this.storageService.saveToken(result.data.token, result.data.tokenLifeTimeMinutes);

      this.authGuard.loginComplete(result.data.account);
      this.message$.next(null);
    }
    else
    {
      this.handleIncident(result.error);
    }
  }

  public getError(control: FormControl): string
  {
    if (control.hasError('required'))
    {
      return 'This field cannot be empty.';
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
