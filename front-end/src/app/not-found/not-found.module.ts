import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from './not-found/not-found.component';
import { NotFoundRoutingModule } from './not-found.routing.module';

import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [CommonModule, NotFoundRoutingModule, SharedModule],
  declarations: [NotFoundComponent],
})
export class NotFoundModule {}
