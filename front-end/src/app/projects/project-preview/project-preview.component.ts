import { Component, Input } from '@angular/core';
import { ProjectPreview } from '../../shared/project-preview.model';

@Component({
  selector: 'app-project-preview',
  templateUrl: './project-preview.component.html',
  styleUrls: ['./project-preview.component.scss'],
})
export class ProjectPreviewComponent {
  @Input()
  public project: ProjectPreview;

  public createRouterLink(): string {
    return '/project/' + this.project.code;
  }
}
