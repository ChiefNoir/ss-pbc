import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AuthGuard } from './guards/authGuard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})

export class AppComponent
{
  public authGuard: AuthGuard;
  public router: Router;

  public constructor(titleService: Title, authGuard: AuthGuard, router: Router)
  {
    this.authGuard = authGuard;
    this.router = router;

    titleService.setTitle(environment.siteName);
  }

}
