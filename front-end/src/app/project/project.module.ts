import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectComponent } from './project/project.component';
import { ProjectRoutingModule } from './project.routing.module';

import { SharedModule } from '../shared/shared.module';
import { ProjectFullComponent } from './project-full/project-full.component';
import { CarouselComponent } from './carousel/carousel.component';

@NgModule({
  imports: [CommonModule, ProjectRoutingModule, SharedModule],
  declarations: [ProjectComponent, ProjectFullComponent, CarouselComponent],
})
export class ProjectModule { }
