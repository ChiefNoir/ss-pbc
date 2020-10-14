import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from 'src/app/core/auth.guard';
import { TextMessages } from 'src/app/shared/text-messages.resources';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-menu',
  templateUrl: './admin-menu.component.html',
  styleUrls: ['./admin-menu.component.scss'],
})

export class AdminMenuComponent
{
  public textMessages: TextMessages = new TextMessages();

  constructor(public authGuard: AuthGuard, public router: Router)
  {

  }
}
