import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './view/home/home.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';
import { ProjectComponent } from './view/project/project.component';
import { NotFoundComponent } from './view/notfound/notfound.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'projects', component: ProjectsListComponent },
  { path: 'project/:code', component: ProjectComponent },
  { path: 'projects/:category', component: ProjectsListComponent },
  { path: 'projects/:category/:page', component: ProjectsListComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
