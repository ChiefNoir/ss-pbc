import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TextMessages } from 'src/app/shared/text-messages.resources';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
})

export class NotFoundComponent
{
  public textMessages: TextMessages = new TextMessages();

  public constructor(titleService: Title)
  {
    titleService.setTitle(this.textMessages.TitlePageNotFound + environment.siteName);
  }
}
