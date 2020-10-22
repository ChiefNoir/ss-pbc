import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectsRoutingModule } from './projects-routing.module';

import { SharedModule } from '../shared/shared.module';

import { ProjectsListComponent } from './project-list/projects-list.component';
import { FilterCategoryComponent } from './filter-category/filter-category.component';
import { ProjectPreviewComponent } from './project-preview/project-preview.component';
import { ButtonCategoryComponent } from './button-category/button-category.component';

@NgModule({
  imports: [
    CommonModule,
    ProjectsRoutingModule,
    SharedModule
  ],
  declarations: [
    ProjectsListComponent,
    FilterCategoryComponent,
    ProjectPreviewComponent,
    ButtonCategoryComponent,
  ],
})
export class ProjectsModule {}
