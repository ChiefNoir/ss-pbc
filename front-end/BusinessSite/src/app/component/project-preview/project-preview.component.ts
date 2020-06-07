import { Component, Input } from '@angular/core';

import { Project } from 'src/app/model/Project';

@Component({
  selector: 'app-project-preview',
  templateUrl: './project-preview.component.html',
  styleUrls: ['./project-preview.component.scss'],
})

export class ProjectPreviewComponent {
  @Input()
  public project: Project;

  public createRouterLink(): string {
    return '/project/' + this.project.code;
  }
}
