import { Component, Input } from '@angular/core';
import { TextMessages } from 'src/app/resources/text-messages.resources';

import { Project } from 'src/app/model/Project';

@Component({
  selector: 'app-project-full',
  templateUrl: './project-full.component.html',
  styleUrls: ['./project-full.component.scss'],
})

export class ProjectFullComponent
{
  @Input()
  public project: Project;

  public textMessages: TextMessages = new TextMessages();
}
