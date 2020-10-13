import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProjectsListComponent } from './project-list/projects-list.component';

const routes: Routes = [
  { path: 'projects', component: ProjectsListComponent },
  { path: 'projects/:category', component: ProjectsListComponent },
];

export const ProjectsRoutingModule: ModuleWithProviders<RouterModule> = RouterModule.forChild(
  routes
);
