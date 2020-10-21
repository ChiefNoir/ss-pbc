import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IntroductionComponent } from './introduction/introduction.component';
import { IntroductionRoutingModule } from './introduction.routing.module';

import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [CommonModule, IntroductionRoutingModule, SharedModule],
  declarations: [IntroductionComponent],
})
export class IntroductionModule {}
