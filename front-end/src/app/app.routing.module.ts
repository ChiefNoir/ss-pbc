import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./introduction/introduction.module').then(x => x.IntroductionModule),
  },
  {
    path: 'projects',
    loadChildren: () => import('./projects/projects.module').then(x => x.ProjectsModule),
  },
  {
    path: 'project',
    loadChildren: () => import('./project/project.module').then(x => x.ProjectModule),
  },
  {
    path: 'login',
    loadChildren: () => import('./auth/auth.module').then(x => x.AuthModule),
  },
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(x => x.AdminModule),
  },
  {
    path: '**',
    loadChildren: () => import('./not-found/not-found.module').then(x => x.NotFoundModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
