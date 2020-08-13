import { Component } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { RequestResult } from 'src/app/model/RequestResult';

import { AuthService } from 'src/app/service/auth.service';
import { Title } from '@angular/platform-browser';

import { environment } from 'src/environments/environment';
import { Identity } from 'src/app/model/Identity';
import { StorageService } from 'src/app/service/storage.service';
import { AuthGuard } from 'src/app/guards/authGuard';


@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss'],
})

export class AdminLoginComponent {
  private router: Router;
  private authGuard: AuthGuard;
  private storageService: StorageService;


  public serviceMessage$: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  private authService: AuthService;

  public login: FormControl = new FormControl('', [Validators.required]);
  public password: FormControl = new FormControl('', [Validators.required]);

  public constructor(authService: AuthService, authGuard: AuthGuard, router: Router, titleService: Title, storageService: StorageService) {
    this.authService = authService;
    this.router = router;
    this.storageService = storageService;
    this.authGuard = authGuard;

    titleService.setTitle(environment.siteName);
  }

  public doLogin(): void {
    if (!this.login.valid || !this.password.valid) {
      return;
    }

    this.authService.login(this.login.value, this.password.value)
                    .then
                    (
                      (result) => this.handleLoginResult(result),
                      (error) => this.handleLoginError(error)
                    );
  }

  private handleLoginResult(result: RequestResult<Identity>): void
  {
    if (result.isSucceed)
    {
      this.storageService.saveToken(result.data.token, result.data.tokenLifeTimeMinutes);

      this.authGuard.loginComplete(result.data.account);
      this.serviceMessage$.next(null);
    }
    else
    {
      this.handleLoginError(result?.errorMessage);
    }
  }

  private handleLoginError(error: string): void {
    console.log(error);
    this.serviceMessage$.next(error);
  }

  public getError(control: FormControl): string {
    if (control.hasError('required')) {
      return 'This field cannot be empty.';
    }
  }
}
