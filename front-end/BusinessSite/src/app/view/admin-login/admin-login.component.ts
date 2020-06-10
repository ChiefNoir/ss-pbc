import { Component } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { RequestResult } from 'src/app/model/RequestResult';

import { AuthService } from 'src/app/service/auth.service';
import { Title } from '@angular/platform-browser';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss'],
})
export class AdminLoginComponent {
  private router: Router;

  public serviceMessage$: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  private authService: AuthService;

  public login: FormControl = new FormControl('', [Validators.required]);
  public password: FormControl = new FormControl('', [Validators.required]);

  public constructor(authService: AuthService, router: Router, titleService: Title) {
    this.authService = authService;
    this.router = router;

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

  private handleLoginResult(result: RequestResult<boolean>): void {
    if (result.isSucceed) {
    } else {
      this.handleLoginError(result?.errorMessage);
    }
  }

  private handleLoginError(error: string): void {
    console.log(error);
    if(error){
      
    this.serviceMessage$.next(error);
    } else{
      this.serviceMessage$.next('asdasd');
      console.log(error);
    }
  }

  public getError(control: FormControl): string {
    if (control.hasError('required')) {
      return 'This field cannot be empty.';
    }
  }
}
