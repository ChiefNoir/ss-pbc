import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
})

export class MessageComponent
{
  @Input()
  public message: MessageDescription;
}

export enum MessageType
{
  Info = 0,
  Error = 1,
  Spinner = 2,
}

export class MessageDescription
{
  public text?: string;
  public type: MessageType;
}
