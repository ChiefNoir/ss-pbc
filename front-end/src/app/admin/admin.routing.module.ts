import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from 'src/app/core/auth.guard';

import { AdminLoginComponent } from './login/admin-login.component';

import { AdminEditProjectsComponent } from './edit-project/admin-edit-projects.component';
import { AdminEditIntroductionComponent } from './edit-introduction/admin-edit-introduction.component';
import { AdminEditCategoriesComponent } from './edit-category/admin-edit-categories.component';
import { AdminEditAccountsComponent } from './edit-account/admin-edit-accounts.component';
import { AdminInformationComponent } from './information/admin-information.component';

const routes: Routes = [
    { path: 'login', component: AdminLoginComponent },
    { path: 'admin', component: AdminInformationComponent, canActivate: [AuthGuard] },
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

export const AdminRoutingModule: ModuleWithProviders<RouterModule> = RouterModule.forChild(
  routes
);
