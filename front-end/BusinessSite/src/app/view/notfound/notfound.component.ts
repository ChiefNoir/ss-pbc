import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { StaticNames } from 'src/app/common/StaticNames';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-not-found',
  templateUrl: './notfound.component.html',
  styleUrls: ['./notfound.component.scss'],
})

export class NotFoundComponent
{
  public staticNames: StaticNames = new StaticNames();

  public constructor(titleService: Title)
  {
    titleService.setTitle(this.staticNames.TitlePageNotFound + environment.siteName);
  }
}
