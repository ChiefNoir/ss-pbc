import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RootComponent } from '../project/root/root.component';

import { ProjectsListComponent } from './project-list/projects-list.component';

const routes: Routes = [
  {
    path: '',
    component: RootComponent,
    pathMatch: 'prefix',
    children: [
      {
        path: '',
        component: ProjectsListComponent
      },
      {
        path: ':category',
        component: ProjectsListComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectsRoutingModule { }
