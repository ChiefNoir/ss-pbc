import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProjectComponent } from './project.component';

const routes: Routes = [
    { path: 'project/:code', component: ProjectComponent },
];

export const ProjectRoutingModule: ModuleWithProviders<RouterModule> = RouterModule.forChild(
  routes
);
