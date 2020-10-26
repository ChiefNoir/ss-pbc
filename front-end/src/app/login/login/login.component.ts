import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';

import { RequestResult } from '../../shared/request-result.interface';
import { Incident } from '../../shared/incident.interface'
import { MessageDescription, MessageType} from '../../shared/message/message.component';

import { Identity } from '../../shared/identity.interface';
import { AuthService } from '../auth.service';
import { AuthGuard } from '../../auth.guard';
import { ResourcesService } from '../../resources.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public login: FormControl = new FormControl('', [Validators.required]);
  public password: FormControl = new FormControl('', [Validators.required]);

  public constructor(
    private authService: AuthService,
    private authGuard: AuthGuard,
    private router: Router,
    titleService: Title,
    public textMessages: ResourcesService
  ) {
    titleService.setTitle(
      this.textMessages.TitlePageLogin + environment.siteName
    );
  }

  public ngOnInit(): void {
    if (this.authGuard.isLoggedIn()) {
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
      this.message$.next(null);

      this.authGuard.loginComplete(result.data);
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
