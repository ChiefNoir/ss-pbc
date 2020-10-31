import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import { IntroductionComponent } from './introduction/introduction.component';
import { IntroductionRoutingModule } from './introduction.routing.module';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    IntroductionRoutingModule
  ],
  declarations: [
    IntroductionComponent
  ],
})
export class IntroductionModule {}
