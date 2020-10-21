import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { MaterialModules } from '../#imports/material.modules';

import { ButtonContactComponent } from './button-contact/button-contact.component';
import { MessageComponent } from './message/message.component';
import { PaginatorComponent } from './paginator/paginator.component';
import { ButtonExternalUrlComponent } from './button-external-url/button-external-url.component';
// import { } from '';


@NgModule({
  imports: [
    CommonModule,
    MaterialModules,
    FlexLayoutModule
  ],
  declarations: [
      ButtonContactComponent,
      MessageComponent,
      PaginatorComponent,
      ButtonExternalUrlComponent
    ],
  exports: [
    MaterialModules,
    FlexLayoutModule,
    ButtonContactComponent,
    MessageComponent,
    PaginatorComponent,
    ButtonExternalUrlComponent
  ]
})
export class SharedModule {}
