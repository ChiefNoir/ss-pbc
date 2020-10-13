import { Component, OnInit } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { RequestResult, Incident } from 'src/app/model/RequestResult';

import { AuthService } from 'src/app/core/auth.service';
import { Title } from '@angular/platform-browser';

import { environment } from 'src/environments/environment';
import { Identity } from 'src/app/model/Identity';
import { AuthGuard } from 'src/app/guards/auth.guard';
import {
  MessageDescription,
  MessageType,
} from 'src/app/component/message/message.component';
import { TextMessages } from 'src/app/resources/text-messages.resources';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss'],
})
export class AdminLoginComponent implements OnInit {
  private authGuard: AuthGuard;
  private authService: AuthService;
  private router: Router;

  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<
    MessageDescription
  >(null);
  public login: FormControl = new FormControl('', [Validators.required]);
  public password: FormControl = new FormControl('', [Validators.required]);
  public textMessages: TextMessages = new TextMessages();

  public constructor(
    authService: AuthService,
    authGuard: AuthGuard,
    router: Router,
    titleService: Title
  ) {
    this.authService = authService;
    this.authGuard = authGuard;
    this.router = router;

    titleService.setTitle(
      this.textMessages.TitlePageLogin + environment.siteName
    );
  }

  public async ngOnInit(): Promise<void> {
    await this.authGuard.checkIsLogged();

    if (this.authGuard.isLoggedIn$.value) {
      this.router.navigate(['/admin']);
    }
  }

  public doLogin(): void {
    if (!this.login.valid || !this.password.valid) {
      return;
    }

    this.authService.login(this.login.value, this.password.value).then(
      (result) => this.handleLoginResult(result),
      (reject) => this.handleError(reject)
    );
  }

  private handleLoginResult(result: RequestResult<Identity>): void {
    if (result.isSucceed) {
      this.authGuard.loginComplete(result.data);
      this.message$.next(null);

      this.router.navigate(['/admin']);
    } else {
      this.handleIncident(result.error);
    }
  }

  public getError(control: FormControl): string {
    if (control.hasError('required')) {
      return this.textMessages.ErrorFieldCannotBeEmpty;
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
