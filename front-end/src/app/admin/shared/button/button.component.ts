import { Component, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss'],
})
export class ButtonComponent {
  @Input()
  public isDisabled: boolean;

  @Output()
  public OnClick: EventEmitter<void> = new EventEmitter();

  public btnClick() {
    this.OnClick.emit();
  }
}
