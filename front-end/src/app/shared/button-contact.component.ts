import { Component, Input } from '@angular/core';
import { ExternalUrl } from 'src/app/shared/external-url.model';

@Component({
  selector: 'app-button-contact',
  templateUrl: './button-contact.component.html',
  styleUrls: ['./button-contact.component.scss']
})

export class ButtonContactComponent
{
  @Input()
  public externalUrl: ExternalUrl;
}
