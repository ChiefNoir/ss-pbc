import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// import { IntroductionRoutingModule } from './introduction/introduction.routing.module';
// import { ProjectsRoutingModule } from './projects/projects-routing.module';
// import { ProjectRoutingModule } from './project/project.routing.module';
// import { NotFoundRoutingModule } from './not-found/not-found.routing.module';
// import { AdminRoutingModule } from './admin/admin.routing.module';

const routes: Routes =
[
  {
    path: '',
    loadChildren: () => import('./introduction/introduction.module').then(m => m.IntroductionModule)
  },
  {
    path: 'projects',
    loadChildren: () => import('./projects/projects.module').then(m => m.ProjectsModule)
  },
  {
    path: 'projects/:category',
    loadChildren: () => import('./projects/projects.module').then(m => m.ProjectsModule)
  },
  {
    path: 'project',
    loadChildren: () => import('./project/project.module').then(m => m.ProjectModule)
  },
  {
    path: 'project/:code',
    loadChildren: () => import('./project/project.module').then(m => m.ProjectModule)
  },
  {
    path: '**',
    loadChildren: () => import('./not-found/not-found.module').then(m => m.NotFoundModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    // AdminRoutingModule,
    // IntroductionRoutingModule,
    // ProjectsRoutingModule,
    // ProjectRoutingModule,
    // NotFoundRoutingModule,
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
