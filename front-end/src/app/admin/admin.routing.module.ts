import { NgModule } from '@angular/core';
import { RouterModule, Route } from '@angular/router';

import { AuthGuard } from '../core/services/auth.guard';

import { Roles } from './roles.enum';

import { AdminComponent } from './admin/admin.component';
import { AdminAccountsComponent } from './admin-accounts/admin-accounts.component';
import { AdminCategoriesComponent } from './admin-categories/admin-categories.component';
import { AdminInformationComponent } from './admin-information/admin-information.component';
import { AdminIntroductionComponent } from './admin-introduction/admin-introduction.component';
import { AdminProjectsComponent } from './admin-projects/admin-projects.component';

const routes: Route[] = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    pathMatch: 'prefix',
    data: {
      expectedRoles: [Roles.Admin, Roles.Demo],
    },
    children: [
      {
        path: '',
        component: AdminInformationComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: [Roles.Admin, Roles.Demo],
        }
      },
      {
        path: 'introduction',
        component: AdminIntroductionComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: [Roles.Admin, Roles.Demo],
        }
      },
      {
        path: 'projects',
        component: AdminProjectsComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: [Roles.Admin, Roles.Demo],
        }
      },
      {
        path: 'categories',
        component: AdminCategoriesComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: [Roles.Admin, Roles.Demo],
        }
      },
      {
        path: 'accounts',
        component: AdminAccountsComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: [Roles.Admin],
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
