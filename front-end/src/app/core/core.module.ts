import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './components/navigation/navigation.component';
import { NavigationCompactComponent } from './components/navigation-compact/navigation-compact.component';

import { SharedModule } from '../shared/shared.module';

import { ResourcesService } from '../core/services/resources.service';
import { AuthGuard } from '../core/services/auth.guard';
import { PublicService } from '../core/services/public.service';
import { StorageService } from '../core/services/storage.service';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
  ],
  declarations: [
    HeaderComponent,
    FooterComponent,
    NavigationComponent,
    NavigationCompactComponent
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    NavigationComponent,
    NavigationCompactComponent,
  ],
  providers: [
    ResourcesService,
    PublicService,
    StorageService,
    AuthGuard
  ]
})
export class CoreModule {}
