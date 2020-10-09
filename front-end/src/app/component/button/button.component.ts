import { Component, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})

export class ButtonComponent
{
  @Input()
  isDisabled: boolean = false;

  @Output()
  OnClick = new EventEmitter();

  public btnClick()
  {
    this.OnClick.emit();
  }
}
