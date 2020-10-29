import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { MaterialModule } from '../material/material.module';

import { ButtonComponent } from './components/button/button.component';
import { ButtonContactComponent } from './components/button-contact/button-contact.component';
import { MessageComponent } from './components/message/message.component';
import { PaginatorComponent } from './components/paginator/paginator.component';
import { ButtonExternalUrlComponent } from './components/button-external-url/button-external-url.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextFieldModule } from '@angular/cdk/text-field';

import { OnlyIntDirective } from './directives/only-int.directive';

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
      ButtonExternalUrlComponent,
      OnlyIntDirective
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
    TextFieldModule,
    OnlyIntDirective
  ],
})
export class SharedModule {}
