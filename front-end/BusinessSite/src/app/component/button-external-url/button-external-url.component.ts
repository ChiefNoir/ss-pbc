import { Component, Input } from '@angular/core';
import { ExternalUrl } from 'src/app/model/ExternalUrl';

@Component({
  selector: 'app-button-external-url',
  templateUrl: './button-external-url.component.html',
  styleUrls: ['./button-external-url.component.scss']
})

export class ButtonExternalUrlComponent {
  @Input()
  public externalUrl: ExternalUrl;
}
