import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './view/home/home.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';
import { ProjectComponent } from './view/project/project.component';
import { NotFoundComponent } from './view/notfound/notfound.component';
import { AdminLoginComponent } from './view/admin-login/admin-login.component';
import { AdminProjectEditorComponent } from './view/admin-project-editor/admin-project-editor.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'projects', component: ProjectsListComponent },
  { path: 'login', component: AdminLoginComponent },
  { path: 'admin/project_editor', component: AdminProjectEditorComponent },
  { path: 'project/:code', component: ProjectComponent },
  { path: 'projects/:category', component: ProjectsListComponent },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule {}
