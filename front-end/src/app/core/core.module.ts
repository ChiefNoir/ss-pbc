import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './navigation/navigation.component';
import { NavigationCompactComponent } from './navigation-compact/navigation-compact.component';

import { SharedModule } from '../shared/shared.module';

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
  ]
})
export class CoreModule {}
