import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './view/admin/admin.component';
import { AdminEditAccountsComponent } from './view/admin-edit-accounts/admin-edit-accounts.component';
import { AdminEditCategoriesComponent } from './view/admin-edit-categories/admin-edit-categories.component';
import { AdminEditIntroductionComponent } from './view/admin-edit-introduction/admin-edit-introduction.component';
import { AdminEditProjectsComponent } from './view/admin-edit-projects/admin-edit-projects.component';
import { AdminLoginComponent } from './view/admin-login/admin-login.component';

import { AuthGuard } from './core/auth.guard';

import { IntroductionRoutingModule } from './introduction/introduction-routing.module';
import { ProjectsRoutingModule } from './projects/projects-routing.module';
import { ProjectRoutingModule } from './project/project.routing.module';
import { NotFoundRoutingModule } from './not-found/not-found.routing.module';

const routes: Routes = [
  { path: 'login', component: AdminLoginComponent },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
  {
    path: 'admin/editor/projects',
    component: AdminEditProjectsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/editor/categories',
    component: AdminEditCategoriesComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/editor/accounts',
    component: AdminEditAccountsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/editor/introduction',
    component: AdminEditIntroductionComponent,
    canActivate: [AuthGuard],
  }
];

@NgModule({
  imports: [
    IntroductionRoutingModule,
    ProjectsRoutingModule,
    ProjectRoutingModule,
    NotFoundRoutingModule,
    RouterModule.forRoot(routes),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
