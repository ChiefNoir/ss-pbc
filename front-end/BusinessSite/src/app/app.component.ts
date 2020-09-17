import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AuthGuard } from './guards/authGuard';
import { StaticNames } from './common/StaticNames';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})

export class AppComponent
{
  public authGuard: AuthGuard;
  public router: Router;
  public staticNames = StaticNames;

  public constructor(titleService: Title, authGuard: AuthGuard, router: Router)
  {
    this.authGuard = authGuard;
    this.router = router;

    titleService.setTitle(environment.siteName);
  }

}
