import { NgModule } from '@angular/core';
import { RouterModule, Route } from '@angular/router';

import { AuthGuard } from '../../app/core/auth.guard';

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
      expectedRoles: ['admin', 'demo'],
    },
    children: [
      {
        path: '',
        component: AdminInformationComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'introduction',
        component: AdminIntroductionComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'projects',
        component: AdminProjectsComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'categories',
        component: AdminCategoriesComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'accounts',
        component: AdminAccountsComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin'],
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
