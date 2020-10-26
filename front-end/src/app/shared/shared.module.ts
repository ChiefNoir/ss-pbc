import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { MaterialModule } from '../material/material.module';

import { ButtonComponent } from './button/button.component';
import { ButtonContactComponent } from './button-contact/button-contact.component';
import { MessageComponent } from './message/message.component';
import { PaginatorComponent } from './paginator/paginator.component';
import { ButtonExternalUrlComponent } from './button-external-url/button-external-url.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextFieldModule } from '@angular/cdk/text-field';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FlexLayoutModule
  ],
  declarations: [
    ButtonComponent,
      ButtonContactComponent,
      MessageComponent,
      PaginatorComponent,
      ButtonExternalUrlComponent
    ],
  exports: [
    MaterialModule,
    FlexLayoutModule,
    ButtonComponent,
    ButtonContactComponent,
    MessageComponent,
    PaginatorComponent,
    ButtonExternalUrlComponent,
    FormsModule,
    ReactiveFormsModule,
    TextFieldModule
  ]
})
export class SharedModule {}
