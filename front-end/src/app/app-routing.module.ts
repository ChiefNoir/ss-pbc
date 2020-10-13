import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IntroductionRoutingModule } from './introduction/introduction-routing.module';
import { ProjectsRoutingModule } from './projects/projects-routing.module';
import { ProjectRoutingModule } from './project/project.routing.module';
import { NotFoundRoutingModule } from './not-found/not-found.routing.module';
import { AdminRoutingModule } from './admin/admin.routing.module';

const routes: Routes = [

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    AdminRoutingModule,
    IntroductionRoutingModule,
    ProjectsRoutingModule,
    ProjectRoutingModule,
    NotFoundRoutingModule,
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
