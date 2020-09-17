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
  public constructor(titleService: Title)
  {
    titleService.setTitle(StaticNames.TitlePageNotFound + environment.siteName);
  }
}
