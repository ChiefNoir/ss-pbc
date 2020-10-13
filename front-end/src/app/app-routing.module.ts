import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './view/admin/admin.component';
import { AdminEditAccountsComponent } from './view/admin-edit-accounts/admin-edit-accounts.component';
import { AdminEditCategoriesComponent } from './view/admin-edit-categories/admin-edit-categories.component';
import { AdminEditIntroductionComponent } from './view/admin-edit-introduction/admin-edit-introduction.component';
import { AdminEditProjectsComponent } from './view/admin-edit-projects/admin-edit-projects.component';
import { AdminLoginComponent } from './view/admin-login/admin-login.component';
import { IntroductionComponent } from './view/introduction/introduction.component';
import { NotFoundComponent } from './view/notfound/notfound.component';
import { ProjectComponent } from './view/project/project.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';

import { AuthGuard } from './guards/auth.guard';

const routes: Routes =
[
  { path: '', component: IntroductionComponent, pathMatch: 'full' },
  { path: 'projects', component: ProjectsListComponent },
  { path: 'login', component: AdminLoginComponent },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
  { path: 'admin/editor/projects',  component: AdminEditProjectsComponent, canActivate: [AuthGuard] },
  { path: 'admin/editor/categories', component: AdminEditCategoriesComponent, canActivate: [AuthGuard] },
  { path: 'admin/editor/accounts', component: AdminEditAccountsComponent, canActivate: [AuthGuard] },
  { path: 'admin/editor/introduction', component: AdminEditIntroductionComponent, canActivate: [AuthGuard] },
  { path: 'project/:code', component: ProjectComponent },
  { path: 'projects/:category', component: ProjectsListComponent },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule {}
