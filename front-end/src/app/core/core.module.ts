import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './components/navigation/navigation.component';
import { NavigationCompactComponent } from './components/navigation-compact/navigation-compact.component';

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
