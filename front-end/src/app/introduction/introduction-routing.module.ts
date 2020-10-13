import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IntroductionComponent } from './introduction/introduction.component';

const routes: Routes = [
  { path: '', component: IntroductionComponent, pathMatch: 'full' },
];

export const IntroductionRoutingModule: ModuleWithProviders<RouterModule> = RouterModule.forChild(
  routes
);
