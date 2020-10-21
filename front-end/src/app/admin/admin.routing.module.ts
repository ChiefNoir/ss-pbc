import { ModuleWithProviders } from '@angular/core';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, Route } from '@angular/router';

import { AuthGuard } from '../../app/core/auth.guard';


import { AdminEditProjectsComponent } from './edit-project/admin-edit-projects.component';
import { AdminEditIntroductionComponent } from './edit-introduction/admin-edit-introduction.component';
import { AdminEditCategoriesComponent } from './edit-category/admin-edit-categories.component';
import { AdminEditAccountsComponent } from './edit-account/admin-edit-accounts.component';
import { AdminInformationComponent } from './information/admin-information.component';
import { AdminComponent } from './admin/admin.component';

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
        component: AdminEditIntroductionComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'projects',
        component: AdminEditProjectsComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'categories',
        component: AdminEditCategoriesComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
      {
        path: 'accounts',
        component: AdminEditAccountsComponent,
        canActivateChild: [AuthGuard],
        data: {
          expectedRoles: ['admin', 'demo'],
        }
      },
    ]
  }
  // {
  //   path: '/editor/projects',
  //   component: AdminEditProjectsComponent,
  //   canActivate: [AuthGuard],
  //   data: {
  //     expectedRoles: ['admin', 'demo'],
  //   },
  // }
      // {
      //   path: '/editor/categories',
      //   component: AdminEditCategoriesComponent,
      //   canActivate: [AuthGuard],
      //   data: {
      //     expectedRoles: ['admin', 'demo'],
      //   },
      // },
      // {
      //   path: '/editor/accounts',
      //   component: AdminEditAccountsComponent,
      //   canActivate: [AuthGuard],
      //   data: {
      //     expectedRoles: ['admin'],
      //   },
      // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
