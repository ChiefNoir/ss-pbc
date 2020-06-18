import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-spinner-text',
  templateUrl: './spinner-text.component.html',
  styleUrls: ['./spinner-text.component.scss']
})

export class SpinnerTextComponent {
  @Input()
  public spinnerText: string;
}
