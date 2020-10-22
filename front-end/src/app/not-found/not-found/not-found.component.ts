import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ResourcesService } from '../../core/resources.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
})
export class NotFoundComponent {
  public constructor(
    titleService: Title,
    public textMessages: ResourcesService
  ) {
    titleService.setTitle(
      this.textMessages.TitlePageNotFound + environment.siteName
    );
  }
}
